using Application.Features.Identity.Users;
using Application.Features.Identity.Users.Commands;
using Application.Features.Identity.Users.Queries;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : BaseApiController
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUserAsync([FromBody] CreateUserRequest createUserRequest)
        {
            var response = await Sender.Send(new CreateUserCommand { CreateUser = createUserRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("update")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Users)]
        public async Task<IActionResult> UpdateUserDetailsAsync([FromBody] UpdateUserRequest updateUserRequest)
        {
            var response = await Sender.Send(new UpdateUserCommand { UpdateUser = updateUserRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut("update-status")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Users)]
        public async Task<IActionResult> UpdateUserStatusAsync([FromBody] ChangeUserStatusRequest changeUserStatusRequest)
        {
            var response = await Sender.Send(new UpdateUserStatusCommand { ChangeUserStatus = changeUserStatusRequest });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpPut("update-roles/{userId}")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.UserRoles)]
        public async Task<IActionResult> UpdateUserRolesAsync([FromBody] UserRolesRequest userRolesRequest, string userId)
        {
            var response = await Sender.Send(new UpdateUserRolesCommand { UserRoles = userRolesRequest, UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
        [HttpDelete("delete/{userId}")]
        [ShouldHavePermission(SchoolAction.Delete, SchoolFeature.Users)]
        public async Task<IActionResult> DeleteUserAsync(string userId)
        {
            var response = await Sender.Send(new DeleteUserCommand { UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Users)]
        public async Task<IActionResult> GetUsersAsync()
        {
            var response = await Sender.Send(new GetAllUsersQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("{userId}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Users)]
        public async Task<IActionResult> GetUserByIdAsync(string userId)
        {
            var response = await Sender.Send(new GetUserByIdQuery { UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("permissions/{userId}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.RoleClaims)]
        public async Task<IActionResult> GetUserPermissionsAsync(string userId)
        {
            var response = await Sender.Send(new GetUserPermissionsQuery { UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }

        [HttpGet("user-roles/{userId}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.UserRoles)]
        public async Task<IActionResult> GetUserRolesAsync(string userId)
        {
            var response = await Sender.Send(new GetUserRolesQuery { UserId = userId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return NotFound(response);
        }
    }
}
