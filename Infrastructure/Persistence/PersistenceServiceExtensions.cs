using Infrastructure.Persistence.Context;
using Infrastructure.Persistence.DbInitilizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class PersistenceServiceExtensions
    {
        public static IServiceCollection AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddDbContext<ApplicationDbContext>(options => options
                    .UseSqlServer(configuration.GetConnectionString("DefaultConnection")))
                .AddTransient<ITenantDbInitilizer, TenantDbInitilizer>()
                .AddTransient<ApplicationDbInitilizer>();
        }

        public static async Task AddInitilizeDatabaseAsync(this IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
        {
            using var scope = serviceProvider.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ITenantDbInitilizer>()
                .InitilizeDatabaseAsync(cancellationToken);
        }
    }
}
