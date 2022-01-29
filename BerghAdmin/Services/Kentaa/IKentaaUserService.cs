using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;
using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.Services.Kentaa;

public interface IKentaaUserService
{
    void AddKentaaUser(KM.User user);
    void AddKentaaUsers(IEnumerable<KM.User> users);
    bool Exist(BihzUser bihzUser);
    IEnumerable<BihzUser>? GetAll();
    BihzUser? GetById(int id);
    BihzUser? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(BihzUser bihzUser);
}
