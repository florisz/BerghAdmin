using BerghAdmin.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services;

public class PersoonServiceWithFactory : IPersoonService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PersoonService> _logger;

    public PersoonServiceWithFactory(ApplicationDbContext dbContext, ILogger<PersoonService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task DeletePersoonAsync(int id)
    {
        _logger.LogDebug("Delete persoon {persoonId}", id);

        using var dbContext = _dbContextFactory.CreateDbContext();
        var persoon = dbContext.Personen?.FirstOrDefault(x => x.Id == id);
        if (persoon != null)
        {
            dbContext.Personen?.Remove(persoon);
            await dbContext.SaveChangesAsync();

            _logger.LogInformation("Persoon with naam {volledigeNaam} deleted", persoon.VolledigeNaam);
        }
    }

    public Persoon? GetByActionId(int actionId)
    {
        _logger.LogDebug("Get persoon by action id {actionId}", actionId);

        using var dbContext = _dbContextFactory.CreateDbContext();
        var persoon = dbContext
                .Personen?
                .AsNoTracking()
                .SingleOrDefault(p => p.BihzActie != null &&
                                        p.BihzActie.Id == actionId);

        _logger.LogInformation("Persoon (id={PersoonId}) with naam {volledigeNaam} retrieved by actionId {actionId} was {result}", 
                persoon?.Id, persoon?.VolledigeNaam, actionId, persoon == null ? "NOT Ok" : "Ok");

        return persoon;
    }

    public Persoon? GetById(int id)
        => GetById(id, false);

    public Persoon? GetById(int id, bool tracked = false)
    {
        Persoon? persoon;

        _logger.LogDebug("Get persoon by id {Id}", id);

        using var dbContext = _dbContextFactory.CreateDbContext();
        if (tracked) {
            persoon = dbContext
                .Personen?
                .Include(p => p.Rollen)
                .Include(e => e.FietsTochten)
                .SingleOrDefault(x => x.Id == id);
        }
        else
        {
            persoon = dbContext
                .Personen?
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.Id == id);
        }

        _logger.LogInformation("Persoon with naam {volledigeNaam} retrieved by id {id} was {result}",
                persoon?.VolledigeNaam, id, persoon == null ? "NOT Ok" : "Ok");

        return persoon;
    }

    public Persoon? GetByEmailAdres(string emailAdres)
    {
        _logger.LogDebug("Get persoon by email adres {emailAdres}", emailAdres);

        using var dbContext = _dbContextFactory.CreateDbContext();
        var persoon = dbContext
                .Personen?
                .AsNoTracking()
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.EmailAdres == emailAdres);

        _logger.LogInformation("Persoon (id={PersoonId}) with naam {volledigeNaam} retrieved by emailadres {emailAdres} was {result}",
                persoon?.Id, persoon?.VolledigeNaam, emailAdres, persoon == null ? "NOT Ok" : "Ok");

        return persoon;
    }

    public List<Persoon>? GetPersonen()
    {
        _logger.LogDebug("Get alle personen");

        using var dbContext = _dbContextFactory.CreateDbContext();
        var personen = dbContext
            .Personen?
            .AsNoTracking()
            .Include(p => p.Rollen)
            .Include(p => p.Donaties)
            .Include(p => p.FietsTochten)
            .ToList();

        _logger.LogInformation("Get alle personen returned {count} results", personen == null? 0 : personen.Count);

        return personen;
    }

    public List<Persoon>? GetFietsersEnBegeleiders()
    {
        _logger.LogDebug("Get alle fietsers en begeleiders");

        using var dbContext = _dbContextFactory.CreateDbContext();
        var personen = dbContext
                .Personen?
                .AsNoTracking()
                .Where(p => p.Rollen.Any(r => r.Id == Convert.ToInt32(RolTypeEnum.Fietser) || r.Id == Convert.ToInt32(RolTypeEnum.Begeleider)))
                .OrderBy(p => p.Achternaam)
                .ToList();

        _logger.LogInformation("Get alle fietsers en begeleiders returned {count} results", personen == null ? 0 : personen.Count);

        return personen;
    }


    public async Task SavePersoonAsync(Persoon persoon)
    {
        _logger.LogDebug("Save persoon with name {volledigeNaam}", persoon.VolledigeNaam);
        using var dbContext = _dbContextFactory.CreateDbContext();
        var debugView = dbContext.ChangeTracker.DebugView.LongView;
        if (persoon.Id == 0)
        {
            dbContext
                .Personen?
                .Add(persoon);

            _logger.LogInformation("Persoon with naam {volledigeNaam} was added", persoon.VolledigeNaam);
        }
        else
        {
            dbContext
                .Personen?
                .Update(persoon);

            _logger.LogInformation("Persoon with naam {volledigeNaam} was updated", persoon.VolledigeNaam);
        }
        await dbContext.SaveChangesAsync();
    }
}
