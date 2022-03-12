using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    Donatie? GetById(int id);
    Donatie? GetByKentaaId(int kentaaDonatieId);
    IEnumerable<Donatie>? GetAll();
    IEnumerable<Donatie>? GetAll(Donateur donateur);
    ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur);
    ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie, Donateur donateur);
    ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie);
    void Save(Donatie donatie);
}
