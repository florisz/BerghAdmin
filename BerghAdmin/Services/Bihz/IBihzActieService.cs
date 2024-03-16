using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Bihz;

public interface IBihzActieService
{
    Task AddAsync(BihzActie action);
    Task AddAsync(IEnumerable<BihzActie> actions);
    bool Exist(BihzActie bihzActie);
    IEnumerable<BihzActie>? GetAll();
    BihzActie? GetById(int id);
    BihzActie? GetByKentaaId(int kentaaId);
    Task<ErrorCodeEnum> SaveAsync(BihzActie bihzActie);
}
