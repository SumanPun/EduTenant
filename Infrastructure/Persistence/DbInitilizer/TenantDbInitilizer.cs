using Finbuckle.MultiTenant;
using Finbuckle.MultiTenant.Abstractions;
using Infrastructure.Persistence.Context;
using Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.DbInitilizer
{
    internal class TenantDbInitilizer(TenantDbContext tenantDbContext, IServiceProvider serviceProvider) : ITenantDbInitilizer
    {
        private readonly TenantDbContext _tenantDbContext = tenantDbContext;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task InitilizeDatabaseAsync(CancellationToken cancellationToken)
        {
            await InitilizeDatabaseWithTenantAsync(cancellationToken);

            foreach(var tenant in await _tenantDbContext.TenantInfo.ToListAsync(cancellationToken))
            {
                await InitilizeApplicationDbForTenantAsync(tenant,cancellationToken);
            }
        }

        private async Task InitilizeDatabaseWithTenantAsync(CancellationToken cancellationToken)
        {
            if (await _tenantDbContext.TenantInfo.FindAsync([TenancyConstants.Root.Id], cancellationToken: cancellationToken) is null)
            {
                var rootTenant = new EduTenantInfo
                {
                    Id = TenancyConstants.Root.Id,
                    Identifier = TenancyConstants.Root.Name,
                    Name = TenancyConstants.Root.Name,
                    AdminEmail = TenancyConstants.Root.Email,
                    IsActive = true,
                    ValidUpTo = DateTime.UtcNow.AddYears(1)
                };

                await _tenantDbContext.TenantInfo.AddAsync(rootTenant, cancellationToken);
                await _tenantDbContext.SaveChangesAsync(cancellationToken);
            }
        }


        private async Task InitilizeApplicationDbForTenantAsync(EduTenantInfo tenant, CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            _serviceProvider.GetRequiredService<IMultiTenantContextSetter>()
                .MultiTenantContext = new MultiTenantContext<EduTenantInfo>()
                {
                    TenantInfo = tenant
                };

            await _serviceProvider.GetRequiredService<ApplicationDbInitilizer>()
                .InitilizeDatabaseAsync(cancellationToken);

        }
    }
}
