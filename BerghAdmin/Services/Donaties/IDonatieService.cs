using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    DonatieBase? GetById(int id);
    IEnumerable<DonatieBase>? GetAll();
    IEnumerable<DonatieBase>? GetAll(Donateur donateur);
    ErrorCodeEnum AddFactuur(DonatieBase donatie, Factuur factuur);
    ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie, Donateur donateur);
    ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie);
    void Save(DonatieBase donatie);
}
