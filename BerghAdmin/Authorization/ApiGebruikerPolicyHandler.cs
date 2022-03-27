using Microsoft.AspNetCore.Authorization;

namespace BerghAdmin.Authorization;
public class ApiGebruikerPolicyHandler : AuthorizationHandler<IsApiGebruikerRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsApiGebruikerRequirement requirement)
    {
        return Task.CompletedTask;
    }
}
