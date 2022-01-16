using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace BerghAdmin.Authorization;
public class BeheerGolfersPolicyHandler : AuthorizationHandler<IsGolfersBeheerderRequirement>
{
    public static Claim Claim
        => new("role", "beheergolfers");
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsGolfersBeheerderRequirement requirement)
    {
        if (context.User.HasClaim(Claim.Type, Claim.Value))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
