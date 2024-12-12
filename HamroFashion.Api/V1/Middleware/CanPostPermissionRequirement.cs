using HamroFashion.Api.V1.Data.Configurations;
using HamroFashion.Api.V1.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace HamroFashion.Api.V1.Middleware
{
    public class CanPostPermissionRequirement : IAuthorizationRequirement
    {
    }
    public class CanPostPermissionRequirementHandler : AuthorizationHandler<CanPostPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanPostPermissionRequirement requirement)
        {
            var hasPerm = context.User.HasPermission(SeedData.CanPost);

            if (hasPerm)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
