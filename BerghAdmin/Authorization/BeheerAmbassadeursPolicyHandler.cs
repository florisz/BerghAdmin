using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace BerghAdmin.Authorization;
public class BeheerAmbassadeursPolicyHandler : AuthorizationHandler<IsAmbassadeursBeheerderRequirement>
{
    public static Claim Claim
        => new("role", "beheerambassadeurs");
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAmbassadeursBeheerderRequirement requirement)
    {
        if (context.User.HasClaim(Claim.Type, Claim.Value))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
