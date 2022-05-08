using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BerghAdmin.Authorization;

namespace BerghAdmin.Services.UserManagement;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private const string DEFAULT_PASSWORD = "qwerty@123";

    public UserService(UserManager<User> userManager)
    {
        this._userManager = userManager;
    }

    public Task DeleteUserAsync(string naam)
    {
        throw new NotImplementedException();
    }

    public async Task<User?> GetUserAsync(string naam)
    {
        var result = await this._userManager.FindByNameAsync(naam);

        return result;
    }

    public async Task InsertUserAsync(string naam, Claim[] claims)
    {
        var user = new User
        {
            CurrentPersoonId = null,
            Name = naam,
            UserName = naam,
            Email = $"{naam}@berghinhetzadel.nl",
            AccessFailedCount = 0,
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = null,
            PhoneNumber = "",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false,
        };

        var result = await this._userManager.CreateAsync(user, DEFAULT_PASSWORD);
        if (result.Succeeded)
        {
            foreach (var claim in claims)
            {
                result = await this._userManager.AddClaimAsync(user, claim);
                if (!result.Succeeded)
                {
                    throw new ApplicationException($"Claim {claim.Type} could not be added to user {user.Name}");
                }
            }
            await this._userManager.UpdateAsync(user);
        }
    }

    public Task InsertUserAsync(string naam, Claim[] claims, Persoon persoon)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(string naam, Claim[] claims)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(string naam, Claim[] claims, Persoon persoon)
    {
        throw new NotImplementedException();
    }

}
