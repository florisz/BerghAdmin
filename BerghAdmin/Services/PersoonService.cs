using BerghAdmin.DbContexts;
using BerghAdmin.Services.Evenementen;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BerghAdmin.Services;

public class PersoonService : IPersoonService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IRolService _rolService;
    private readonly ILogger<PersoonService> _logger;

    public PersoonService(ApplicationDbContext dbContext, IRolService rolService, IFietstochtenService fietstochtenService, ILogger<PersoonService> logger)
    {
        _dbContext = dbContext;
        _rolService = rolService;
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

    public Task<Persoon?> GetByActionId(int actionId)
    {
        _logger.LogDebug($"Get persoon by action id:{actionId}");

        var persoon = _dbContext
                .Personen?
                .SingleOrDefault(p => p.BihzActie != null &&
                                        p.BihzActie.Id == actionId);

        _logger.LogDebug($"Persoon (id={persoon?.Id}) with naam {persoon?.VolledigeNaam} retrieved by actionId {actionId} was {((persoon == null) ? "NOT Ok" : "Ok")}");

        return Task.FromResult(persoon);
    }

    public Task<Persoon?> GetById(int id)
    {
        Persoon? persoon;

        _logger.LogDebug($"Get persoon by id {id}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        // add try catch
        try
        {
            persoon = _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .Include(e => e.Fietstochten)
                .SingleOrDefault(x => x.Id == id);

            _logger.LogDebug($"Persoon with naam {persoon?.VolledigeNaam} retrieved by id {id} was {((persoon == null) ? "NOT Ok" : "Ok")}");

            return Task.FromResult(persoon);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error retrieving persoon with id {id}");
            throw;
        }
    }

    public Task<Persoon?> GetByEmailAdres(string emailAdres)
    {
        _logger.LogDebug($"Get persoon by email adres {emailAdres}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var persoon = _dbContext
                .Personen?
                .Include(p => p.Rollen)
                .SingleOrDefault(x => x.EmailAdres == emailAdres);

        _logger.LogDebug($"Persoon (id={persoon?.Id}) with naam {persoon?.VolledigeNaam} retrieved by emailadres {emailAdres} was {((persoon == null) ? "NOT Ok" : "Ok")}");

        return Task.FromResult(persoon);
    }

    public Task<List<Persoon>> GetPersonen()
    {
        _logger.LogDebug($"Get alle personen; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var personen = _dbContext
            .Personen?
            .Include(p => p.Rollen)
            .Include(p => p.Donaties)
            .Include(p => p.Fietstochten)
            .ToList();

        _logger.LogDebug($"Get alle personen returned {((personen == null) ? 0 : personen.Count)} personen");
        
        return Task.FromResult(personen ?? new List<Persoon>());
    }

    public Task<PersoonListItem[]> GetFietstochtDeelnemers()
    {
        _logger.LogDebug($"Get alle fietsers en begeleiders; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var personen = _dbContext
                .Personen?
                .Where(p => p.Rollen.Any(r => r.Id == Convert.ToInt32(RolTypeEnum.Fietser) || r.Id == Convert.ToInt32(RolTypeEnum.Begeleider)))
                .OrderBy(p => p.Achternaam)
                .Select(p => new PersoonListItem() { Id = p.Id, VolledigeNaamMetRollenEnEmail = p.VolledigeNaamMetRollenEnEmail })
                .ToArray();

        _logger.LogDebug($"Get alle fietsers en begeleiders returned {((personen == null) ? 0 : personen.Count())} personen");

        return Task.FromResult(personen ?? new PersoonListItem[] { });
    }


    public async Task SavePersoonAsync(Persoon persoon)
    {
        _logger.LogDebug($"SaveAsync persoon with name {persoon.VolledigeNaam}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");
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

    //
    // Helper function to set rollen based on the selection in a listbox
    //
    public void SetRollen(Persoon persoon, List<RolListItem> rolListItems)
    {
        persoon.Rollen.Clear();
        foreach (var rolListItem in rolListItems)
        {
            var rol = _rolService.GetRolById((RolTypeEnum)rolListItem.Id);
            if (rol == null)
            {
                throw new ApplicationException($"Rol with id {rolListItem.Id} does not exist");
            }
            persoon.Rollen.Add(rol!);
        }
    }

}
