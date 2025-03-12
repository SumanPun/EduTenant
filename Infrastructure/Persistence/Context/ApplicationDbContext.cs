using Domain.Entities;
using Finbuckle.MultiTenant.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context
{
    public class ApplicationDbContext(ITenantInfo tenantInfo, DbContextOptions<ApplicationDbContext> options) 
        : BaseDbContext(tenantInfo, options)
    {
        public DbSet<School> Schools => Set<School>();
    }

}
