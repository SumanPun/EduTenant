﻿using Infrastructure.Identity.Constants;
using Infrastructure.Identity.Models;
using Infrastructure.Persistence.Context;
using Infrastructure.Tenancy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.DbInitilizer
{
    public class ApplicationDbInitilizer(EduTenantInfo tenant,
        RoleManager<ApplicationRole> roleManager,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext applicationDbContext) 
    {
        private readonly EduTenantInfo _tenant = tenant;
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _applicationDbContext = applicationDbContext;

        public async Task InitilizeDatabaseAsync(CancellationToken cancellationToken)
        {
            if (_applicationDbContext.Database.GetMigrations().Any())
            {
                if((await _applicationDbContext.Database.GetPendingMigrationsAsync(cancellationToken)).Any())
                {
                    await _applicationDbContext.Database.MigrateAsync(cancellationToken);
                }

                if(await _applicationDbContext.Database.CanConnectAsync(cancellationToken))
                {
                    await InitilizeDefaultRolesAsync(cancellationToken);
                    await InitilizeAdminUserAsync();
                }
            }
        }

        private async Task InitilizeDefaultRolesAsync(CancellationToken cancellationToken)
        {
            foreach(string roleName in RoleConstants.DefaultRoles)
            {
                if(await _roleManager.Roles.SingleOrDefaultAsync(role => role.Name == roleName, cancellationToken)
                    is not ApplicationRole incomingRole)
                {
                    incomingRole = new ApplicationRole() { Name = roleName, Description = $"{roleName} Role" };
                    await _roleManager.CreateAsync(incomingRole);
                }

                //assign permissions to newly added role
                if(roleName == RoleConstants.Basic)
                {
                    await AssaignPermissionsToRole(SchoolPermissions.Basic, incomingRole, cancellationToken);
                }
                else if(roleName == RoleConstants.Admin)
                {
                    await AssaignPermissionsToRole(SchoolPermissions.Admin, incomingRole, cancellationToken);
                    if(_tenant.Id == TenancyConstants.Root.Id)
                    {
                        await AssaignPermissionsToRole(SchoolPermissions.Root, incomingRole, cancellationToken);
                    }
                }

            }
        }

        private async Task InitilizeAdminUserAsync()
        {

            if (string.IsNullOrEmpty(_tenant.AdminEmail))
            {
                return;
            }

            if(await _userManager.Users.FirstOrDefaultAsync(u => u.Email == _tenant.AdminEmail)
                is not ApplicationUser adminUser)
            {
                adminUser = new ApplicationUser()
                {
                    FirstName = TenancyConstants.FirstName,
                    LastName = TenancyConstants.LastName,
                    Email = _tenant.AdminEmail,
                    UserName = _tenant.AdminEmail,
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    NormalizedEmail = _tenant.AdminEmail.ToUpperInvariant(),
                    NormalizedUserName = _tenant.AdminEmail.ToUpperInvariant(),
                    IsActive = true
                };

                var password = new PasswordHasher<ApplicationUser>();
                adminUser.PasswordHash = password.HashPassword(adminUser, TenancyConstants.DefaultPassword);
                await _userManager.CreateAsync(adminUser);
            }

            if(!await _userManager.IsInRoleAsync(adminUser, RoleConstants.Admin))
            {
                //assaign user to admin role
                await _userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
            }
        }

        private async Task AssaignPermissionsToRole(
            IReadOnlyList<SchoolPermission> rolePermissions,
            ApplicationRole currentRole, CancellationToken cancellationToken)
        {
            var currentClaims = await _roleManager.GetClaimsAsync(currentRole);
            foreach(var rolePermission in rolePermissions)
            {
                if(!currentClaims.Any(c => c.Type == ClaimConstants.Permission && c.Value == rolePermission.Name))
                {
                    await applicationDbContext.RoleClaims.AddAsync(new IdentityRoleClaim<string>
                    {
                        RoleId = currentRole.Id,
                        ClaimType = ClaimConstants.Permission,
                        ClaimValue = rolePermission.Name
                    }, cancellationToken);

                    await applicationDbContext.SaveChangesAsync(cancellationToken);
                }
            }
        }
    }
}
