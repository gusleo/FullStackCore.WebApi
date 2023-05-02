using Core.Data.Infrastructure;

namespace FullStackCore.WebApi.CronJob;

/// <summary>
/// List of cron jobs
/// </summary>
public class CronJobHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICoreContext _coreContext;

    /// <summary>
    /// Initialize
    /// </summary>
    /// <param name="serviceProvider"></param>
    public CronJobHostedService(IServiceProvider serviceProvider, ICoreContext context)
    {
        _serviceProvider = serviceProvider;
        _coreContext = context;
    }

    /// <summary>
    /// Call all cron jobs and run by schedule
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            //var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            //var cronJobSettings = await dbContext.CronJobSettings.FirstAsync();

            //var apiService = scope.ServiceProvider.GetRequiredService<ApiService>();
            //RecurringJob.AddOrUpdate("CallApiAndSaveResult", () => apiService.CallApiAndSaveResult(), cronJobSettings.CronExpression);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
