using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication5.Auth;

public class OwnerOrAdminHandler : AuthorizationHandler<OwnerOrAdminRequirement, IOwnedResource>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context, OwnerOrAdminRequirement requirement, IOwnedResource resource)
    {
        if (context.User.IsInRole(Roles.Admin))
        {
            context.Succeed(requirement);
        }
        else if (int.TryParse(context.User.FindFirstValue(ClaimTypes.NameIdentifier), out var userId)
                 && userId == resource.OwnerId)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
