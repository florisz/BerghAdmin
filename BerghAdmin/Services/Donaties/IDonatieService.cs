using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    ErrorCodeEnum Save(KentaaDonatie donatie);
    bool Exist(KentaaDonatie donatie);
    Donatie? GetById(int id);
    KentaaDonatie? GetByKentaaId(int kentaaId);
    IEnumerable<KentaaDonatie> GetAll<KentaaDonatie>();
    IEnumerable<Donatie> GetAll();
    ErrorCodeEnum AddDonateur(Donatie donatie, Donateur persoon);
    ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur);
}
