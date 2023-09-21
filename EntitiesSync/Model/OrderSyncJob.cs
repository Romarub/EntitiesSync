using Quartz;

namespace EntitiesSync.Model;

[DisallowConcurrentExecution]
class OrderSyncJob : IJob
{
    // TODO https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/more-about-jobs.html#jobdatamap:~:text=If%20you%20add,custom%20JobFactory.
    public const string BatchSizeKey = "batchSize";
    public const string LastProcessedIdKey = "lastProcessedId";

    // TODO попробовать делегат с именованными параметрами
    private readonly Func<long, int, IEnumerable<OrderDto>> _getOrders;
    private readonly Action<IReadOnlyCollection<OrderDto>> _syncOrders;

    public OrderSyncJob(Func<long, int, IEnumerable<OrderDto>> getOrders, Action<IReadOnlyCollection<OrderDto>> syncOrders)
    {
        _getOrders = getOrders;
        _syncOrders = syncOrders;
    }

    public Task Execute(IJobExecutionContext context)
    {
        try
        {
            Console.WriteLine("Test job execution");
            var dataMap = context.MergedJobDataMap;
            
            // test scenario -> cast exception (no field or another type)
            var batchSize = 2; // dataMap.GetInt(BatchSizeKey);
            IReadOnlyCollection<OrderDto> orders = Array.Empty<OrderDto>();
            do
            {
                var lastProcessedId = dataMap.GetLong(LastProcessedIdKey);
                Console.WriteLine($"lastProcessedId: {lastProcessedId}");
                orders = _getOrders(lastProcessedId, batchSize).ToArray().AsReadOnly();
                if (!orders.Any())
                    break;

                _syncOrders(orders);
                var lastId = orders.Last().OrderId;
                dataMap[LastProcessedIdKey] = lastId;
                Console.WriteLine($"lastId: {lastId}");
            } while (orders.Any());
            Console.WriteLine("finishec cycle");
            Console.WriteLine();
        }
        catch (Exception ex)
        {
            throw new JobExecutionException(ex);
        }


        return Task.CompletedTask;
    }
}

record class OrderDto(int OrderId, int ClientId, DateTime OrderDateTimeUtc);
