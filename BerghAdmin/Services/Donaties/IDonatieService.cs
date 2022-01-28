using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    Donatie? GetById(int id);
    IEnumerable<Donatie> GetAll();
    ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur);
    ErrorCodeEnum AddKentaaDonatie(KentaaDonation kentaaDonatie, Donateur persoon);
    ErrorCodeEnum AddKentaaDonatie(KentaaDonation kentaaDonatie);
    void Save(Donatie donatie);
}
