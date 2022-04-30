using BerghAdmin.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services;

public class PersoonService : IPersoonService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PersoonService> _logger;

    public PersoonService(ApplicationDbContext context, ILogger<PersoonService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public void DeletePersoon(int id)
    {
        _logger.LogDebug("Delete persoon {persoonId}", id);

        var persoon = _dbContext.Personen?.FirstOrDefault(x => x.Id == id);
        if (persoon != null)
        {
            _dbContext.Personen?.Remove(persoon);
            _dbContext.SaveChanges();

            _logger.LogInformation("Persoon with naam {volledigeNaam} deleted", persoon.VolledigeNaam);
        }
    }

    public Persoon? GetByActionId(int actionId)
    {
        _logger.LogDebug("Get persoon by action id {actionId}", actionId);

        var persoon = _dbContext
                        .Personen?
                        .SingleOrDefault(p => p.BihzActie != null &&
                                              p.BihzActie.Id == actionId);

        _logger.LogInformation("Persoon (id={PersoonId}) with naam {volledigeNaam} retrieved by actionId {actionId} was {result}", 
                persoon?.Id, persoon?.VolledigeNaam, actionId, persoon == null ? "NOT Ok" : "Ok");

        return persoon;
    }

    public Persoon? GetById(int id)
    {
        _logger.LogDebug("Get persoon by id {Id}", id);

        var persoon = _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.Id == id);

        _logger.LogInformation("Persoon with naam {volledigeNaam} retrieved by id {id} was {result}",
                persoon?.VolledigeNaam, id, persoon == null ? "NOT Ok" : "Ok");

        return persoon;
    }

    public Persoon? GetByEmailAdres(string emailAdres)
    {
        _logger.LogDebug("Get persoon by email adres {emailAdres}", emailAdres);

        var persoon = _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.EmailAdres == emailAdres);

        _logger.LogInformation("Persoon (id={PersoonId}) with naam {volledigeNaam} retrieved by emailadres {emailAdres} was {result}",
                persoon?.Id, persoon?.VolledigeNaam, emailAdres, persoon == null ? "NOT Ok" : "Ok");

        return persoon;
    }

    public List<Persoon>? GetPersonen()
    {
        _logger.LogDebug("Get alle personen");

        var personen = _dbContext
            .Personen?
            .Include(p => p.Rollen)
            .Include(p => p.Donaties)
            .ToList();

        _logger.LogInformation("Get alle personen returned {count} results", personen == null? 0 : personen.Count);

        return personen;
    }

    public List<Persoon>? GetFietsers()
    {
        _logger.LogDebug("Get alle fietsers");

        var personen = _dbContext
                .Personen?
                .Where(p => p.Rollen.Any(r => r.Id == RolTypeEnum.Fietser))
                .ToList();

        _logger.LogInformation("Get alle fietsers returned {count} results", personen == null ? 0 : personen.Count);

        return personen;
    }

    public void SavePersoon(Persoon persoon)
    {
        _logger.LogDebug("Save persoon with name {volledigeNaam}", persoon.VolledigeNaam);

        if (persoon.Id == 0) 
        {
            _dbContext
                .Personen?
                .Add(persoon);

            _logger.LogInformation("Persoon with naam {volledigeNaam} was added", persoon.VolledigeNaam);
        }
        else
        { 
            _dbContext
                .Personen?
                .Update(persoon);

            _logger.LogInformation("Persoon with naam {volledigeNaam} was updated", persoon.VolledigeNaam);
        }
        _dbContext.SaveChanges();
    }

}
