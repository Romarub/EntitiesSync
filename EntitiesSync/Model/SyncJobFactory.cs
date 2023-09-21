using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace EntitiesSync.Model;

class SyncJobFactory : IJobFactory
{
    private readonly IServiceProvider _serviceProvider;

    public SyncJobFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        // return _serviceProvider.GetRequiredService(bundle.JobDetail.JobType) as IJob;
        return _serviceProvider.GetRequiredService<OrderSyncJob>();
    }

    public void ReturnJob(IJob job)
    {
        var disposable = job as IDisposable;
        disposable?.Dispose();
    }
}
