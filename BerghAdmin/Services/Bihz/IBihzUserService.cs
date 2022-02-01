using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Bihz;

public interface IBihzUserService
{
    void Add(BihzUser user);
    void Add(IEnumerable<BihzUser> users);
    bool Exist(BihzUser bihzUser);
    IEnumerable<BihzUser>? GetAll();
    BihzUser? GetById(int id);
    BihzUser? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(BihzUser bihzUser);
}
