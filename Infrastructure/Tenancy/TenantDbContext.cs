using Finbuckle.MultiTenant.Stores;
using Infrastructure.Persistence.DbConfiguration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Tenancy
{
    public class TenantDbContext(DbContextOptions<TenantDbContext> options)
        : EFCoreStoreDbContext<EduTenantInfo>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder
                .Entity<EduTenantInfo>()
                .ToTable("Tenants", SchemaNames.MultiTenancy);
        }
    }   
}
