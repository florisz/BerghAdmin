using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    Donatie? GetById(int id);
    IEnumerable<Donatie> GetAll();
    ErrorCodeEnum AddDonateur(Donatie donatie, Donateur persoon);
    ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur);
    ErrorCodeEnum AddKentaaDonatie(Donatie donatie, KentaaDonation kentaaDonatie);
}
