using Core.Service.Infrastructure;
using Core.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using Scaffolding.Auth;

namespace Core.Services.Extensions;

public static class ServiceLayerExtension
{
    public static void AddServiceLayer(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserDetailService, UserDetailService>();
        services.AddScoped<ILogEntryService, LogEntryService>();

    }
}
