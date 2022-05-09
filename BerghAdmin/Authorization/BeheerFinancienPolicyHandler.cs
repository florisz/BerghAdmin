using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace BerghAdmin.Authorization;
public class BeheerFinancienPolicyHandler : AuthorizationHandler<IsFinancienBeheerderRequirement>
{
    public static Claim Claim
        => new("role", "beheerfinancien");
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsFinancienBeheerderRequirement requirement)
    {
        if (context.User.HasClaim(Claim.Type, Claim.Value))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
