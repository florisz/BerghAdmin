using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public class DonatieService : IDonatieService
{
    public ErrorCodeEnum AddDonateur(Donatie donatie, Donateur persoon)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Donatie>? GetAll<T>()
    {
        throw new NotImplementedException();
    }

    public Donatie? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Donatie? GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum Save(Donatie donatie)
    {
        throw new NotImplementedException();
    }
}
