using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;
using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.Services.Bihz;

public interface IBihzActieService
{
    void AddBihzAction(KM.Action action);
    void AddBihzActions(IEnumerable<KM.Action> actions);
    bool Exist(BihzActie bihzActie);
    IEnumerable<BihzActie>? GetAll();
    BihzActie? GetById(int id);
    BihzActie? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(BihzActie bihzActie);
}
