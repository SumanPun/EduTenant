using Application.Features.Commands;
using Application.Features.Tenancy;

namespace Infrastructure.Tenancy
{
    public class TenantService : ITenantService
    {
        public Task<string> CreateTenantAsync(CreateTenantRequest createTenant)
        {
            throw new NotImplementedException();
        }
    }
}
