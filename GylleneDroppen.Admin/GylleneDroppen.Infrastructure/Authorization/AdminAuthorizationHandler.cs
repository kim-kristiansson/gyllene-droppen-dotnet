using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace GylleneDroppen.Infrastructure.Authorization;

public class AdminRequirement : IAuthorizationRequirement;

public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        AdminRequirement requirement)
    {
        var user = context.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        if (user.IsInRole("Admin") ||
            user.HasClaim(ClaimTypes.Role, "Admin") ||
            user.HasClaim("role", "Admin"))
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