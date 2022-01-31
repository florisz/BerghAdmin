using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Bihz;

public interface IBihzActieService
{
    void Add(BihzActie action);
    void Add(IEnumerable<BihzActie> actions);
    bool Exist(BihzActie bihzActie);
    IEnumerable<BihzActie>? GetAll();
    BihzActie? GetById(int id);
    BihzActie? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(BihzActie bihzActie);
}
