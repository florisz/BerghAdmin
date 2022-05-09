using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BerghAdmin.Authorization;

namespace BerghAdmin.Services.UserManagement;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;
    private const string DEFAULT_PASSWORD = "Qwerty@123";

    public UserService(UserManager<User> userManager)
    {
        this._userManager = userManager;
    }

    public Task DeleteUserAsync(string naam)
    {
        throw new NotImplementedException();
    }

    public async Task<User> GetUserAsync(string naam)
        => await this._userManager.FindByNameAsync(naam);

    public IList<User> GetUsers()
        =>_userManager.Users.ToList();

    public async Task<IList<Claim>> GetUserClaimsAsync(string naam)
    {
        var user = await GetUserAsync(naam);
        if (user == null)
            return new List<Claim>();

        var userClaims = await _userManager.GetClaimsAsync(user);

        return userClaims;
    }

    public async Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam)
        => await InsertUserAsync(naam, Array.Empty<Claim>(), null );

    public async Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam, Persoon? persoon)
        => await InsertUserAsync(naam, Array.Empty<Claim>(), persoon);

    public async Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam, Claim[] claims)
        => await InsertUserAsync(naam, claims, null);


    public async Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam, Claim[] claims, Persoon? persoon)
    {
        var user = new User
        {
            CurrentPersoonId = (persoon == null)? null : persoon.Id,
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
        if (!result.Succeeded)
        {
            return result.Errors;
        }
        else
        {
            foreach (var claim in claims)
            {
                result = await this._userManager.AddClaimAsync(user, claim);
                if (!result.Succeeded)
                {
                    return result.Errors;
                }
            }
            await this._userManager.UpdateAsync(user);
        }

        return null;
    }

    public Task<IEnumerable<IdentityError>?> UpdateUserAsync(string naam, Claim[] claims)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<IdentityError>?> UpdateUserAsync(string naam, Claim[] claims, Persoon? persoon)
    {
        throw new NotImplementedException();
    }


}
