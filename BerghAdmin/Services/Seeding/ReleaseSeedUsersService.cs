using BerghAdmin.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BerghAdmin.Services.Seeding;

public class ReleaseSeedUsersService : ISeedUsersService
{
    private readonly UserManager<User> _userManager;

    public ReleaseSeedUsersService(UserManager<User> userManager)
    {
        this._userManager = userManager;
    }

    public async Task SeedUsersData()
    {
        if (DatabaseHasUsers())
        {
            return;
        }

        await InsertUser("admin", new Claim[] {
            AdministratorPolicyHandler.Claim,
            BeheerAmbassadeursPolicyHandler.Claim,
            BeheerFietsersPolicyHandler.Claim,
            BeheerFinancienPolicyHandler.Claim,
            BeheerGolfersPolicyHandler.Claim,
            BeheerSecretariaatPolicyHandler.Claim,
        });
        await InsertUser("secretariaat", new Claim[] {
            BeheerAmbassadeursPolicyHandler.Claim,
            BeheerFietsersPolicyHandler.Claim,
            BeheerFinancienPolicyHandler.Claim,
            BeheerGolfersPolicyHandler.Claim,
            BeheerSecretariaatPolicyHandler.Claim,
        });
        await InsertUser("ambassadeursadmin", new Claim[] { BeheerAmbassadeursPolicyHandler.Claim });
        await InsertUser("fietsenadmin", new Claim[] { BeheerFietsersPolicyHandler.Claim });
        await InsertUser("financienadmin", new Claim[] { BeheerFinancienPolicyHandler.Claim });
        await InsertUser("golfadmin", new Claim[] { BeheerGolfersPolicyHandler.Claim });
    }

    private bool DatabaseHasUsers()
        => _userManager.Users.Any();


    private async Task InsertUser(string naam, Claim[] claims)
    {
        var user = new User
        {
            CurrentPersoonId = null,
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

        var result = await this._userManager.CreateAsync(user, "Qwerty@123");
        if (result.Succeeded)
        {
            foreach (var claim in claims)
            {
                await this._userManager.AddClaimAsync(user, claim);
                await this._userManager.UpdateAsync(user);
            }
        }
    }

}
