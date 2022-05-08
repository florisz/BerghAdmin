using BerghAdmin.Authorization;
using System.Security.Claims;

namespace BerghAdmin.Services.UserManagement;

public interface IUserService
{
    Task DeleteUserAsync(string naam);
    Task<User> GetUserAsync(string naam);
    Task InsertUserAsync(string naam, Claim[] claims);
    Task InsertUserAsync(string naam, Claim[] claims, Persoon persoon);
    Task UpdateUserAsync(string naam, Claim[] claims);
    Task UpdateUserAsync(string naam, Claim[] claims, Persoon persoon);
}
