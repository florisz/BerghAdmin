
using Microsoft.AspNetCore.Authorization;

using System.Threading.Tasks;

namespace BerghAdmin.Authorization;
public class AdministratorPolicyHandler : AuthorizationHandler<IsAdministratorRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsAdministratorRequirement requirement)
    {
        if (context.User.HasClaim(c => c.Type == "role" && c.Value == "administrator"))
            context.Succeed(requirement);

        return Task.CompletedTask;
    }
}
