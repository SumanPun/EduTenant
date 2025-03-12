using Infrastructure.Tenancy;

namespace Infrastructure.Persistence.DbInitilizer
{
    internal interface ITenantDbInitilizer
    {
        Task InitilizeDatabaseAsync(CancellationToken cancellationToken);
    }
}
