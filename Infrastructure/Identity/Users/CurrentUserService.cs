using Application.Exceptions;
using Application.Features.Identity.Users;
using System.Security.Claims;

namespace Infrastructure.Identity.Users
{
    public class CurrentUserService : ICurrentUserService
    {
        private ClaimsPrincipal _principle;

        public string Name => _principle.Identity.Name;

        public IEnumerable<Claim> GetUserClaims()
        {
            return _principle.Claims;
        }

        public string GetUserEmail()
        {
            if (IsAuthenticated())
            {
                return _principle.GetEmail();
            }
            return string.Empty;
        }

        public string GetUserId()
        {
            if (IsAuthenticated())
            {
                return _principle.GetUserId();
            }
            return string.Empty;
        }

        public string GetUserTenant()
        {
            if (IsAuthenticated())
            {
                return _principle.GetTenant();
            }
            return string.Empty;
        }

        public bool IsAuthenticated()
            => _principle.Identity.IsAuthenticated;

        public bool IsInRole(string roleName)
            => _principle.IsInRole(roleName);

        public void SetCurrentUser(ClaimsPrincipal principal)
        {
            if(principal is not null)
            {
                _principle = principal;
            }
            throw new ConflictException("Invalid operation on claim.");
        }
    }
}
