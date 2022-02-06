using BerghAdmin.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services;

public class PersoonService : IPersoonService
{
    private readonly ApplicationDbContext _dbContext;

    public PersoonService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void DeletePersoon(int id)
    {
        var persoon = _dbContext.Personen?.FirstOrDefault(x => x.Id == id);
        if(persoon != null)
        {
            _dbContext.Personen?.Remove(persoon);
            _dbContext.SaveChanges();
        }
    }

    public Persoon? GetByActionId(int actionId)
        => _dbContext
                .Personen?
                .SingleOrDefault(p => p.BihzActie != null &&
                                      p.BihzActie.Id == actionId);

    public Persoon? GetById(int id)
        => _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.Id == id);

    public Persoon? GetByEmailAdres(string emailAdres)
        => _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.EmailAdres == emailAdres);

    public List<Persoon>? GetPersonen()
    {
        return _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .ToList();
    }

    public List<Persoon>? GetFietsers()
    {
        return _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .Where(p => p.Rollen.Any(r => r.Id == RolTypeEnum.Fietser))
                .ToList();
    }

    public void SavePersoon(Persoon persoon)
    {
        if (persoon.Id == 0) 
        {
            _dbContext
                .Personen?
                .Add(persoon);
        }
        else
        { 
            _dbContext
                .Personen?
                .Update(persoon);
        }
        _dbContext.SaveChanges();
    }

}
