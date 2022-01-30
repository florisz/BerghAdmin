using BerghAdmin.Data.Kentaa;
using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    DonatieBase? GetById(int id);
    IEnumerable<DonatieBase> GetAll();
    ErrorCodeEnum AddFactuur(DonatieBase donatie, Factuur factuur);
    ErrorCodeEnum AddBihzDonatie(BihzDonatie bihzDonatie, Donateur persoon);
    ErrorCodeEnum AddBihzDonatie(BihzDonatie bihzDonatie);
    void Save(DonatieBase donatie);
}
