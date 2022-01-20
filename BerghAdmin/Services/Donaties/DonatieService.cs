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

    public IEnumerable<Donatie>? GetAll<T>()
    {
        throw new NotImplementedException();
    }

    public Donatie? GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Donatie? GetByKentaaId(int kentaaActionId)
        => _dbContext
            .Donaties?
            .SingleOrDefault(d => d.KentaaActionId == kentaaActionId);

    public Donatie? GetByName(string name)
    {
        throw new NotImplementedException();
    }

    public ErrorCodeEnum Save(Donatie donatie)
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

        return ErrorCodeEnum.Ok;
    }

}
