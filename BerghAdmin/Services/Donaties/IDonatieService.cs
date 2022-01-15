using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public interface IDonatieService
{
    ErrorCodeEnum Save(Donatie donatie);
    Donatie? GetById(int id);
    Donatie? GetByName(string name);
    IEnumerable<Donatie>? GetAll<T>();
    ErrorCodeEnum AddDonateur(Donatie donatie, Donateur persoon);
    ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur);
}
