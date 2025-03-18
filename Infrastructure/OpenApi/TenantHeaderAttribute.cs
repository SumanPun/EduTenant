using Infrastructure.Tenancy;

namespace Infrastructure.OpenApi
{
    public class TenantHeaderAttribute : SwaggerHeaderAttribute
    {
        public TenantHeaderAttribute() 
            : base(TenancyConstants.TenantIdName,
                  "Input your tenant name to access this API.",
                  string.Empty, true)
        {

        }
    }
}
