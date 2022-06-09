using Microsoft.AspNetCore.Authorization;

using System.Security.Claims;

namespace BerghAdmin.Authorization;
public class BeheerSecretariaatPolicyHandler : AuthorizationHandler<IsSecretariaatBeheerderRequirement>
{
    public static Claim Claim
        => new("role", "beheersecretariaat");
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsSecretariaatBeheerderRequirement requirement)
    {
        if (context.User.HasClaim(Claim.Type, Claim.Value))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
