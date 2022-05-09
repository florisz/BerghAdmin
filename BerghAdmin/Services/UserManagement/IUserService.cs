using BerghAdmin.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BerghAdmin.Services.UserManagement;

public interface IUserService
{
    Task DeleteUserAsync(string naam);
    Task<User> GetUserAsync(string naam);
    IList<User> GetUsers();
    IList<Claim> GetClaims();
    Task<IList<Claim>> GetUserClaimsAsync(string naam);
    Task<IdentityResult> InsertUserAsync(User user, string password);
    Task<IdentityResult> InsertUserAsync(User user, string password, Claim[] claims);
    Task<IdentityResult> UpdateUserAsync(User user);
    Task<IdentityResult> UpdateUserAsync(User user, string password);
    Task<IdentityResult> UpdateUserAsync(User user, Claim[] claims);
}
