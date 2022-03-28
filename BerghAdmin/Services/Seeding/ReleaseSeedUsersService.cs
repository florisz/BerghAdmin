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

        await InsertUser("ict", AdministratorPolicyHandler.Claim);
        await InsertUser("secretaris", AdministratorPolicyHandler.Claim);
        await InsertUser("fietsen", BeheerGolfersPolicyHandler.Claim);
        await InsertUser("golfdagbergh", BeheerGolfersPolicyHandler.Claim);
        await InsertUser("penningmeester", BeheerAmbassadeursPolicyHandler.Claim);
        await InsertUser("ambassadeurbeheer", BeheerAmbassadeursPolicyHandler.Claim);
        await InsertUser("sponsoring", BeheerAmbassadeursPolicyHandler.Claim);
    }

    private bool DatabaseHasUsers()
        => this._userManager.Users.Count() > 0;


    private async Task InsertUser(string naam, Claim claim)
    {
        var user = new User
        {
            CurrentPersoonId = 0, // to be set manually after all data is imported
            Name = naam,
            UserName = $"{naam}@berghinhetzadel.nl",
            Email = $"{naam}@berghinhetzadel.nl",
            AccessFailedCount = 0,
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = null,
            PhoneNumber = "",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false
        };

        var result = await this._userManager.CreateAsync(user, "qwerty@123");
        if (result.Succeeded)
        {
            await this._userManager.AddClaimAsync(user, claim);
        }
    }

}
