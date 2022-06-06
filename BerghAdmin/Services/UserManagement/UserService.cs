using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using BerghAdmin.Authorization;

namespace BerghAdmin.Services.UserManagement;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    public UserService(UserManager<User> userManager)
    {
        this._userManager = userManager;
    }

    public Task DeleteUserAsync(string naam)
    {
        throw new NotImplementedException();
    }

    public IDictionary<string, Claim> GetClaims()
    {
        return new Dictionary<string, Claim>()
        {
            { AdministratorPolicyHandler.Claim.Value, AdministratorPolicyHandler.Claim },
            { BeheerAmbassadeursPolicyHandler.Claim.Value, BeheerAmbassadeursPolicyHandler.Claim },
            { BeheerFietsersPolicyHandler.Claim.Value, BeheerFietsersPolicyHandler.Claim },
            { BeheerFinancienPolicyHandler.Claim.Value, BeheerFinancienPolicyHandler.Claim },
            { BeheerGolfersPolicyHandler.Claim.Value, BeheerGolfersPolicyHandler.Claim },
            { BeheerSecretariaatPolicyHandler.Claim.Value, BeheerSecretariaatPolicyHandler.Claim }
        };
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

    public async Task<IdentityResult> InsertUserAsync(User user, string password)
        => await InsertUserAsync(user, password, Array.Empty<Claim>(), null );

    public async Task<IdentityResult> InsertUserAsync(User user, string password, Persoon? persoon)
        => await InsertUserAsync(user, password, Array.Empty<Claim>(), persoon);

    public async Task<IdentityResult> InsertUserAsync(User user, string password, Claim[] claims)
        => await InsertUserAsync(user, password, claims, null);


    public async Task<IdentityResult> InsertUserAsync(User user, string password, Claim[] claims, Persoon? persoon)
    {
        var result = await this._userManager.CreateAsync(user, password);
        if (!result.Succeeded)
        {
            return result;
        }
        else
        {
            foreach (var claim in claims)
            {
                if (!IsValidClaim(claim))
                {
                    throw new InvalidOperationException($"The claim {claim.Type} with value {claim.Value} and value type {claim.ValueType} can not be added.");
                }
                result = await this._userManager.AddClaimAsync(user, claim);
                if (!result.Succeeded)
                {
                    return result;
                }
            }
            result = await this._userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return result;
            }
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateUserAsync(User user)
    {
        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return result;
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateUserAsync(User user, string password)
    {
        var result = await UpdateUserAsync(user);
        if (!result.Succeeded)
        {
            return result;
        }
        result = await _userManager.RemovePasswordAsync(user);
        if (!result.Succeeded)
        {
            return result;
        }
        result = await _userManager.AddPasswordAsync(user, password);
        if (!result.Succeeded)
        {
            return result;
        }

        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateUserAsync(User user, Claim[] claims)
    {
        // update the user properties
        var result = await this._userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            return result;
        }

        // throw away all old claims
        var oldClaims = await GetUserClaimsAsync(user.Name);
        if (oldClaims.Count > 0)
        {
            result = await this._userManager.RemoveClaimsAsync(user, oldClaims);
            if (!result.Succeeded)
            {
                return result;
            }
        }

        // add all new claims
        foreach (var claim in claims)
        {
            if (!IsValidClaim(claim))
            {
                throw new InvalidOperationException($"The claim {claim.Type} with value {claim.Value} and value type {claim.ValueType} can not be added.");
            }
            result = await this._userManager.AddClaimAsync(user, claim);
            if (!result.Succeeded)
            {
                return result;
            }
        }

        return IdentityResult.Success;
    }

    private bool IsValidClaim(Claim claim)
        => GetClaims().Values.FirstOrDefault(c => c.Value == claim.Value && c.Type == claim.Type) != null;

}
