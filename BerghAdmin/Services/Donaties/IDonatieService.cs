using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    DonatieBase? GetById(int id);
    IEnumerable<DonatieBase> GetAll();
    ErrorCodeEnum AddFactuur(DonatieBase donatie, Factuur factuur);
    ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie, Donateur persoon);
    ErrorCodeEnum ProcessBihzDonatie(BihzDonatie bihzDonatie);
    void Save(DonatieBase donatie);
}
