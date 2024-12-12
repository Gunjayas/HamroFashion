using HamroFashion.Api.V1.Data.Configurations;
using HamroFashion.Api.V1.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace HamroFashion.Api.V1.Middleware
{
    public class CanOrderPermissionRequirement : IAuthorizationRequirement
    {
    }
    public class CanOrderPermissionRequirementHandler : AuthorizationHandler<CanOrderPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CanOrderPermissionRequirement requirement)
        {
            var hasPerm = context.User.HasPermission(SeedData.CanOrderPermissionId);

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
