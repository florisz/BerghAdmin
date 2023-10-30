using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SkiaSharp;
using System;

namespace BerghAdmin.Services;

public class PersoonService : IPersoonService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<PersoonService> _logger;

    public PersoonService(ApplicationDbContext dbContext, ILogger<PersoonService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        logger.LogDebug($"PersoonService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }

    public async Task DeletePersoonAsync(int id)
    {
        _logger.LogDebug($"Delete persoon with id:{id}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var persoon = _dbContext.Personen?.FirstOrDefault(x => x.Id == id);
        if (persoon != null)
        {
            _dbContext.Personen?.Remove(persoon);
            await _dbContext.SaveChangesAsync();

            _logger.LogDebug($"Persoon with naam {persoon.VolledigeNaam} deleted");
        }
    }

    public Persoon? GetByActionId(int actionId)
    {
        _logger.LogDebug($"Get persoon by action id:{actionId}");

        var persoon = _dbContext
                .Personen?
                .AsNoTracking()
                .SingleOrDefault(p => p.BihzActie != null &&
                                        p.BihzActie.Id == actionId);

        _logger.LogDebug($"Persoon (id={persoon?.Id}) with naam {persoon?.VolledigeNaam} retrieved by actionId {actionId} was {((persoon == null)? "NOT Ok" : "Ok")}");

        return persoon;
    }

    public Persoon? GetById(int id)
        => GetById(id, false);

    public Persoon? GetById(int id, bool tracked = false)
    {
        Persoon? persoon;

        _logger.LogDebug($"Get persoon by id {id}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        // add try catch
        try
        {
            if (tracked)
            {
                persoon = _dbContext
                    .Personen?
                    .Include(p => p.Rollen)
                    .Include(e => e.FietsTochten)
                    .SingleOrDefault(x => x.Id == id);
            }
            else
            {
                persoon = _dbContext
                    .Personen?
                    .Include(p => p.Rollen)
                    .Include(e => e.FietsTochten)
                    .SingleOrDefault(x => x.Id == id);
            }

            _logger.LogDebug($"Persoon with naam {persoon?.VolledigeNaam} retrieved by id {id} was {((persoon == null) ? "NOT Ok" : "Ok")}");

            return persoon;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving persoon with id {id}");
            throw;
        }
    }

    public Persoon? GetByEmailAdres(string emailAdres)
    {
        _logger.LogDebug($"Get persoon by email adres {emailAdres}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var persoon = _dbContext
                .Personen?
                .AsNoTracking()
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.EmailAdres == emailAdres);

        _logger.LogDebug($"Persoon (id={persoon?.Id}) with naam {persoon?.VolledigeNaam} retrieved by emailadres {emailAdres} was {((persoon == null) ? "NOT Ok" : "Ok")}");

        return persoon;
    }

    public List<Persoon>? GetPersonen()
    {
        _logger.LogDebug($"Get alle personen; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var personen = _dbContext
            .Personen?
            .AsNoTracking()
            .Include(p => p.Rollen)
            .Include(p => p.Donaties)
            .Include(p => p.FietsTochten)
        .ToList();
        _logger.LogDebug($"Get alle personen returned {((personen == null) ? 0 : personen.Count)} personen");
        return personen;
    }
    public List<Persoon>? GetFietsersEnBegeleiders()
    {
        _logger.LogDebug($"Get alle fietsers en begeleiders; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var personen = _dbContext
                .Personen?
                .AsNoTracking()
                .Where(p => p.Rollen.Any(r => r.Id == Convert.ToInt32(RolTypeEnum.Fietser) || r.Id == Convert.ToInt32(RolTypeEnum.Begeleider)))
                .OrderBy(p => p.Achternaam)
                .ToList();

        _logger.LogDebug($"Get alle fietsers en begeleiders returned {((personen == null) ? 0 : personen.Count)} personen");

        return personen;
    }


    public async Task SavePersoonAsync(Persoon persoon)
    {
        _logger.LogDebug($"Save persoon with name {persoon.VolledigeNaam}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");
        var debugView = _dbContext.ChangeTracker.DebugView.LongView;
        if (persoon.Id == 0)
        {
            _dbContext
                .Personen?
                .Add(persoon);

            _logger.LogInformation($"Persoon with naam {persoon.VolledigeNaam} was added");
        }
        else
        {
            _dbContext
                .Personen?
                .Update(persoon);

            _logger.LogInformation($"Persoon with naam {persoon.VolledigeNaam} was updated");
        }

        await _dbContext.SaveChangesAsync();
    }
}
