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

    public bool Exist(KentaaDonatie donatie)
        => GetByKentaaId(donatie.KentaaDonationId) != null;

    public IEnumerable<Donatie> GetAll()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<KentaaDonatie> GetAll<KentaaDonatie>()
    {
        throw new NotImplementedException();
    }

    public KentaaDonatie? GetById(int id)
       => _dbContext
            .KentaaDonaties?
            .SingleOrDefault(kd => kd.Id == id);

    public KentaaDonatie? GetByKentaaId(int kentaaId)
        => _dbContext
            .KentaaDonaties?
            .SingleOrDefault(kd => kd.KentaaDonationId == kentaaId);


    public ErrorCodeEnum Save(KentaaDonatie donatie)
    {
        try
        {
            if (donatie.Id == 0)
            {
                _dbContext
                    .Donaties?
                    .Add(donatie);
            }
            else
            {
                _dbContext
                    .Donaties?
                    .Update(donatie);
            }

            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            // log exception
            return ErrorCodeEnum.Conflict;
        }

        return ErrorCodeEnum.Ok;
    }

    Donatie? IDonatieService.GetById(int id)
    {
        throw new NotImplementedException();
    }
}
