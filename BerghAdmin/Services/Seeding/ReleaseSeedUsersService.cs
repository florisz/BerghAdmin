using BerghAdmin.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BerghAdmin.Services.Seeding;

public class ReleaseSeedUsersService(UserManager<User> userManager) : ISeedUsersService
{
    public async Task SeedUsersData()
    {
        if (DatabaseHasUsers())
        {
            return;
        }

        await InsertUser("admin", [
            AdministratorPolicyHandler.Claim,
            BeheerAmbassadeursPolicyHandler.Claim,
            BeheerFietsersPolicyHandler.Claim,
            BeheerFinancienPolicyHandler.Claim,
            BeheerGolfersPolicyHandler.Claim,
            BeheerSecretariaatPolicyHandler.Claim,
        ]);
        await InsertUser("secretariaat", [
            BeheerAmbassadeursPolicyHandler.Claim,
            BeheerFietsersPolicyHandler.Claim,
            BeheerFinancienPolicyHandler.Claim,
            BeheerGolfersPolicyHandler.Claim,
            BeheerSecretariaatPolicyHandler.Claim,
        ]);
        await InsertUser("ambassadeursadmin", [BeheerAmbassadeursPolicyHandler.Claim]);
        await InsertUser("fietsenadmin", [BeheerFietsersPolicyHandler.Claim]);
        await InsertUser("financienadmin", [BeheerFinancienPolicyHandler.Claim]);
        await InsertUser("golfadmin", [BeheerGolfersPolicyHandler.Claim]);
    }

    private bool DatabaseHasUsers()
        => userManager.Users.Any();


    private async Task InsertUser(string naam, Claim[] claims)
    {
        var user = new User
        {
            Name = naam,
            UserName = $"{naam}@berghinhetzadel.nl",
            Email = $"{naam}@berghinhetzadel.nl",
            AccessFailedCount = 0,
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = null,
            PhoneNumber = "",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
            LoginCount = 1
        };

        var result = await userManager.CreateAsync(user, "Qwerty@123");
        if (result.Succeeded)
        {
            foreach (var claim in claims)
            {
                await userManager.AddClaimAsync(user, claim);
                await userManager.UpdateAsync(user);
            }
        }
    }

}
