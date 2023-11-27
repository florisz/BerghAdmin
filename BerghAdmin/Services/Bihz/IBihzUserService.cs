using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Bihz;

public interface IBihzUserService
{
    Task AddAsync(BihzUser user);
    Task AddAsync(IEnumerable<BihzUser> users);
    Task<bool> ExistAsync(BihzUser bihzUser);
    Task<List<BihzUser>?> GetAll();
    Task<BihzUser?> GetById(int id);
    Task<BihzUser?> GetByKentaaId(int kentaaId);
    Task SaveAsync(BihzUser bihzUser);
}
