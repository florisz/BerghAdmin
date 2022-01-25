using BerghAdmin.DbContexts;
using BerghAdmin.General;

namespace BerghAdmin.Services.Donaties;

public class DonatieService : IDonatieService
{
    private readonly ApplicationDbContext _dbContext;

    public DonatieService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public ErrorCodeEnum AddDonateur(Donatie donatie, Donateur persoon)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddFactuur(Donatie donatie, Factuur factuur)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum AddKentaaDonatie(Donatie donatie, KentaaDonation kentaaDonatie)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Donatie> GetAll()
    {
        throw new NotImplementedException();
    }

    public Donatie? GetById(int id)
    {
        throw new NotImplementedException();
    }
}
