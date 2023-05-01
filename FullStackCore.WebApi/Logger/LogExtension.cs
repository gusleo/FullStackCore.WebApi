using Core.Service.Infrastructure;
using Core.Service.Services;

namespace FullStackCore.WebApi.Logger;

public static class LogExtension
{
    public static void AddLogger(this IServiceCollection services)
    {
        services.AddLogging();
        services.AddSingleton<ILoggerProvider, DatabaseLoggerProvider>();
    }
}
