using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public interface IKentaaUserService
{
    void AddKentaaUser(KM.User kentaaUser);
    void AddKentaaUsers(IEnumerable<KM.User> kentaaUsers);

    bool Exist(KentaaUser user);
    IEnumerable<KentaaUser>? GetAll();
    KentaaUser? GetById(int id);
    KentaaUser? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(KentaaUser donatie);
}
