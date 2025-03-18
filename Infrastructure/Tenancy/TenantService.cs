using Application.Features.Tenancy;
using Application.Features.Tenancy.Commands;
using Application.Features.Tenancy.Models;
using Finbuckle.MultiTenant;
using Infrastructure.Persistence.DbInitilizer;
using Mapster;

namespace Infrastructure.Tenancy
{
    public class TenantService(IMultiTenantStore<EduTenantInfo> tenantStore, ApplicationDbInitilizer applicationDbInitilizer) : ITenantService
    {
        private readonly IMultiTenantStore<EduTenantInfo> _tenantStore = tenantStore;
        private readonly ApplicationDbInitilizer _applicationDbInitilizer = applicationDbInitilizer;

        public async Task<string> ActivateAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = true;

            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<string> CreateTenantAsync(CreateTenantRequest createTenant, CancellationToken cancellationToken)
        {
            // create tenant in db
            var newTenant = new EduTenantInfo
            {
                Id = createTenant.Identifier,
                Name = createTenant.Name,
                ConnectionString = createTenant.ConnectionString,
                AdminEmail = createTenant.AdminEmail,
                ValidUpTo = createTenant.ValidUpTo,
                IsActive = createTenant.IsActive,
            };

            await _tenantStore.TryAddAsync(newTenant);

            // initilize tenant with users, user roles, roles, role permissions
            try
            {
                await _applicationDbInitilizer.InitilizeDatabaseAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _tenantStore.TryRemoveAsync(createTenant.Identifier);
                throw;
            }

            return newTenant.Id;

        }

        public async Task<string> DeactivateAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.IsActive = false;

            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }

        public async Task<TenantDto> GetTenantByIdAsync(string id)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);

            return tenantInDb.Adapt<TenantDto>();
        }

        public async Task<List<TenantDto>> GetTenantsAsync()
        {
            return (await _tenantStore.GetAllAsync()).Adapt<List<TenantDto>>();
        }

        public async Task<string> UpdateSubscriptionAsync(string id, DateTime newExpiryDate)
        {
            var tenantInDb = await _tenantStore.TryGetAsync(id);
            tenantInDb.ValidUpTo = newExpiryDate;

            await _tenantStore.TryUpdateAsync(tenantInDb);
            return tenantInDb.Id;
        }
    }
}
