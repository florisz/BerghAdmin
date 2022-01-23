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
        var donaties = _dbContext
                        .KentaaDonaties;

        if (donaties == null)
        {
            return Enumerable.Empty<KentaaDonatie>();
        }

        return (IEnumerable<KentaaDonatie>) donaties;
    }

    public Donatie? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public KentaaDonatie? GetByKentaaId(int kentaaId)
        => _dbContext
            .KentaaDonaties?
            .SingleOrDefault(kd => kd.KentaaDonationId == kentaaId);


    public ErrorCodeEnum Save(KentaaDonatie donatie)
    {
        if (!Exist(donatie))
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

        return ErrorCodeEnum.Ok;
    }

}
