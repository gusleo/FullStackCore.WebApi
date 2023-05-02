using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Authorization.Server;

public class Worker : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public Worker(IServiceProvider serviceProvider)
        => _serviceProvider = serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CoreContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        await RegisterApplicationsAsync(scope.ServiceProvider, cancellationToken);
        await RegisterScopesAsync(scope.ServiceProvider, cancellationToken);

        static async Task RegisterApplicationsAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            var manager = provider.GetRequiredService<IOpenIddictApplicationManager>();

            // API
            if (await manager.FindByClientIdAsync("resource_server", cancellationToken) == null)
            {
                var descriptor = new OpenIddictApplicationDescriptor
                {
                    ClientId = "resource_server",
                    ClientSecret = "846B62D0-DEF9-4215-A99D-86E6B8DAB342",
                    Permissions =
                        {
                            Permissions.Endpoints.Introspection
                        }
                };

                await manager.CreateAsync(descriptor, cancellationToken);
            }

            // React JS Hosted
            if (await manager.FindByClientIdAsync("react-js-client", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "react-js-client",
                    ConsentType = ConsentTypes.Explicit,
                    DisplayName = "React Js code PKCE",
                    PostLogoutRedirectUris =
                    {
                        new Uri("https://localhost:44348/signout-callback-oidc")
                    },
                    RedirectUris =
                    {
                        new Uri("https://localhost:44348/signin-oidc")
                    },
                    ClientSecret = "6BC0AA21-500B-4C13-9440-EBBF1872EDB5",
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Logout,
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.ResponseTypes.Code,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Roles,
                        Permissions.Prefixes.Scope + "api"
                    },
                        Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                    }, cancellationToken);
            }

            if (await manager.FindByClientIdAsync("postman", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "postman",
                    ClientSecret = "1FF8E944-0E2E-483C-AAEC-C47A9F1B5C32",
                    DisplayName = "Postman",
                    RedirectUris = { new Uri("https://oauth.pstmn.io/v1/callback") },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,
                        Permissions.Scopes.Email,
                        Permissions.Scopes.Roles,
                        Permissions.Scopes.Profile,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.RefreshToken,

                        Permissions.Prefixes.Scope + "api",
                        Permissions.ResponseTypes.Code
                    }
                }, cancellationToken);
            }

        }

        static async Task RegisterScopesAsync(IServiceProvider provider, CancellationToken cancellationToken)
        {
            var manager = provider.GetRequiredService<IOpenIddictScopeManager>();

            if (await manager.FindByNameAsync("api", cancellationToken) is null)
            {
                await manager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    DisplayName = "Kalbe API access",                    
                    Name = "api",
                    Resources =
                    {
                        "resource_server"
                    }
                }, cancellationToken);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
