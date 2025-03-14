using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Auth
{
    public class ShouldHavePermission : AuthorizeAttribute
    {
        public ShouldHavePermission(string action, string feature)
        {
            Policy = SchoolPermission.NameFor(action, feature);
        }
    }
}
