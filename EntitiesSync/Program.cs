using Quartz;
using Quartz.Impl;
using Microsoft.Extensions.DependencyInjection;

using EntitiesSync.Model;
using EntitiesSync.Infrastructure;

var serviceProvider = new ServiceCollection()
    .AddScoped<OrderSyncJob>()
    .AddSingleton<IOrderRepository, OrderRepository>()
    .AddSingleton<Func<long, int, IEnumerable<OrderDto>>>(sp =>
    {
        return (lastProcessedId, batchSize) =>
            {
            var orders = sp.GetRequiredService<IOrderRepository>().GetOrders()
                .Where(o => o.Id > lastProcessedId)
                .OrderBy(o => o.Id)
                .Take(batchSize)
                .ToArray();

                return orders.Select(o => new OrderDto(o.Id, o.Customer.Id, o.DateTimeUtc));
            };
    })
    .AddSingleton<Action<IReadOnlyCollection<OrderDto>>>(sp =>
    {
        return act => Console.WriteLine($"Orders sent: {act.Count}");
    })
    // .AddQuartz(configure =>
    // {
    //     var jobKey = new JobKey(nameof(OrderSyncJob));

    //     configure
    //         .AddJob<OrderSyncJob>(jobKey)
    //         .AddTrigger(
    //             trigger => trigger.ForJob(jobKey).WithSimpleSchedule(
    //                 schedule => schedule.WithIntervalInSeconds(10).RepeatForever()));
    // })
    .BuildServiceProvider();

var schedulerFactory = new StdSchedulerFactory();
var scheduler = await schedulerFactory.GetScheduler();
scheduler.JobFactory = new SyncJobFactory(serviceProvider);
await scheduler.Start();

var job = JobBuilder.Create<OrderSyncJob>()
    .WithIdentity("job1", "group1")
    .Build();

var trigger = TriggerBuilder.Create()
    .WithIdentity("trigger1", "group1")
    .StartNow()
    .WithSimpleSchedule(x => x
        .WithIntervalInSeconds(10)
        .RepeatForever())
    .Build();

await scheduler.ScheduleJob(job, trigger);

await Task.Delay(TimeSpan.FromSeconds(60));

await scheduler.Shutdown();

Console.WriteLine("Hello, World!");
Console.WriteLine("Test");
