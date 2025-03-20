using Infrastructure.Identity;
using Infrastructure.OpenApi;
using Infrastructure.Persistence;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            return services
                .AddMultiTenancyServices(configuration)
                .AddPersistenceService(configuration)
                .AddIdentityServices()
                .AddPermissions()
                .AddJwtAuthentication()
                .AddOpenApiDocumentation(configuration);
        }

        public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder application)
        {
            return application
                .UseAuthentication()
                .UseCurrentUser()
                .UseMultitenancy()
                .UseAuthorization()
                .UseOpenApiDocumentation();
        }
    }
}
