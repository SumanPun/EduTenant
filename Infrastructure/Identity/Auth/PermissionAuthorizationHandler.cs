using Application.Features.Identity.Users;
using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Identity.Auth
{
    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IUserService _userService;

        public PermissionAuthorizationHandler(IUserService userService)
        {
            _userService = userService;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if(context.User.GetUserId() is { } userId &&
                await _userService.IsPermissionAssignedAsync(userId, requirement.Permission))
            {
                context.Succeed(requirement);
            }
        }
    }
}
