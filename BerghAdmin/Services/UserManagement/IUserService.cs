using BerghAdmin.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BerghAdmin.Services.UserManagement;

public interface IUserService
{
    Task DeleteUserAsync(string naam);
    Task<User> GetUserAsync(string naam);
    IList<User> GetUsers();
    Task<IList<Claim>> GetUserClaimsAsync(string naam);
    Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam);
    Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam, Persoon? persoon);
    Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam, Claim[] claims);
    Task<IEnumerable<IdentityError>?> InsertUserAsync(string naam, Claim[] claims, Persoon? persoon);
    Task<IEnumerable<IdentityError>?> UpdateUserAsync(string naam, Claim[] claims);
    Task<IEnumerable<IdentityError>?> UpdateUserAsync(string naam, Claim[] claims, Persoon? persoon);
}
