﻿using Application.Features.Tenancy.Commands;
using Application.Features.Tenancy.Models;
using Application.Features.Tenancy.Queries;
using Infrastructure.Identity.Auth;
using Infrastructure.Identity.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    public class TenantsController : BaseApiController
    {
        [HttpPost("add")]
        [ShouldHavePermission(SchoolAction.Create, SchoolFeature.Tenants)]
        public async Task<IActionResult> CreateTenantAsync([FromBody] CreateTenantRequest createTenant)
        {
            var response = await Sender.Send(new CreateTenantCommand { CreateTenant = createTenant });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{tenantId}/activate")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Tenants)]
        public async Task<IActionResult> ActivateTenantAsync(string tenantId)
        {
            var response = await Sender.Send(new ActivateTenantCommand { TenantId = tenantId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("{tenantId}/deactivate")]
        [ShouldHavePermission(SchoolAction.Update, SchoolFeature.Tenants)]
        public async Task<IActionResult> DeactivateTenantAsync(string tenantId)
        {
            var response = await Sender.Send(new DeactivateTenantCommand { TenantId = tenantId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPut("upgrade")]
        [ShouldHavePermission(SchoolAction.UpgradeSubscription, SchoolFeature.Tenants)]
        public async Task<IActionResult> UpgradeTenantSubscriptionAsync([FromBody] UpdateTenantSubscriptionRequest updateTenant)
        {
            var response = await Sender.Send(new UpdateTenantSubscriptionCommand { UpdateTenantSubscription = updateTenant });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("{tenantId}")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Tenants)]
        public async Task<IActionResult> GetTenantByIdAsync(string tenantId)
        {
            var response = await Sender.Send(new GetTenantByIdQuery { TenantId = tenantId });
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpGet("all")]
        [ShouldHavePermission(SchoolAction.View, SchoolFeature.Tenants)]
        public async Task<IActionResult> GetTenantsAsync()
        {
            var response = await Sender.Send(new GetTenantsQuery());
            if (response.IsSuccessful)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
