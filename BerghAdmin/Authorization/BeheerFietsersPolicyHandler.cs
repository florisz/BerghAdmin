using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace BerghAdmin.Authorization;
public class BeheerFietsersPolicyHandler : AuthorizationHandler<IsFietsersBeheerderRequirement>
{
    public static Claim Claim
        => new("role", "beheerfietsers");
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsFietsersBeheerderRequirement requirement)
    {
        if (context.User.HasClaim(Claim.Type, Claim.Value) ||
            context.User.HasClaim(AdministratorPolicyHandler.Claim.Type, AdministratorPolicyHandler.Claim.Value))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
