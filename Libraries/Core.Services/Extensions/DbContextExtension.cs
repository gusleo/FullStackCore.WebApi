using Core.Data;
using Core.Data.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services.Extensions
{
    public static class DbContextExtension
    {
        public static void AddDatabaseLayer(this IServiceCollection services, string connectionString){
            services.AddScoped<ICoreContext>(provider => provider.GetService<CoreContext>());
            services.AddScoped<ICoreUnitOfWork, CoreUnitOfWork>();
            services.AddDbContext<CoreContext>(
                opt => {
                    opt.UseSqlServer(connectionString);
                }
            );
        }
    }
}
