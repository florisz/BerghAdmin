using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Evenementen;

public class FietstochtenService : IFietstochtenService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<FietstochtenService> _logger;

    public FietstochtenService(ApplicationDbContext dbContext, ILogger<FietstochtenService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        _logger.LogDebug($"FietstochtenService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }

    public Task<List<Fietstocht>> GetAll()
    {
        var fietstochtenList =
            _dbContext
                .Fietstochten?
                .Include(e => e.Deelnemers)
                .ThenInclude(p => p.Rollen)
                .ToList();

        return Task.FromResult(fietstochtenList ?? new List<Fietstocht>());
    }
    public Task<Fietstocht?> GetById(int id)
    { 
        var fietstocht =_dbContext
            .Fietstochten?
            .FirstOrDefault(e => e.Id == id);

        return Task.FromResult(fietstocht);
    }

    public Task<Fietstocht?> GetByTitel(string? titel)
    {
        var fietstocht =_dbContext
            .Fietstochten?
            .FirstOrDefault(e => e.Titel == titel);

        return Task.FromResult(fietstocht);
    }

    public Task<Fietstocht?> GetByProjectId(int projectId)
    {
        var fietstocht = _dbContext
            .Fietstochten?
            .FirstOrDefault(e => e.KentaaProjectId == projectId);

        return Task.FromResult(fietstocht);
    }
    public async Task<Fietstocht?> GetByProject(BihzProject project)
    {
        if (project == null)
        {
            return null;
        }

        var fietstocht = await GetByProjectId(project.ProjectId);
        if (fietstocht == null)
        {
            fietstocht = await GetByTitel(project.Titel);
        }

        return fietstocht;
    }

    public async Task<ErrorCodeEnum> SaveAsync(Fietstocht fietstocht)
    {
        _logger.LogDebug($"Entering save fietstocht {fietstocht.Titel}");
        if (fietstocht == null)
        {
            throw new ApplicationException("Fietstocht parameter can not be null");
        }

        if (fietstocht.Id == 0)
        {
            if (await GetByTitel(fietstocht.Titel) != null)
            {
                return ErrorCodeEnum.Conflict;
            }

            _dbContext.Fietstochten?.Add(fietstocht);
        }
        else
        {
            _dbContext.Fietstochten?.Update(fietstocht);
        }
        _logger.LogDebug($"In SaveAsync() before save; threadid={Thread.CurrentThread.ManagedThreadId}");

        await _dbContext.SaveChangesAsync();

        return ErrorCodeEnum.Ok;
    }

    public async Task<ErrorCodeEnum> AddDeelnemerAsync(Fietstocht fietstocht, Persoon persoon)
    {
        _logger.LogDebug($"Entering AddAsync deelnemer {persoon.VolledigeNaam} to {fietstocht.Titel}");

        if (fietstocht == null) { throw new ApplicationException("parameter fietstocht can not be null"); }
        if (persoon == null) { throw new ApplicationException("parameter persoon can not be null"); }

        if (fietstocht.Deelnemers?.FirstOrDefault(p => p.Id == persoon.Id) != null)
        {
            return ErrorCodeEnum.Conflict;
        }

        if (fietstocht.Deelnemers == null)
        {
            fietstocht.Deelnemers = new HashSet<Persoon>();
        }

        fietstocht.Deelnemers.Add(persoon);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation($"Deelnemer added {persoon.VolledigeNaam} to {fietstocht.Titel}");
        return ErrorCodeEnum.Ok;
    }

    public async Task<ErrorCodeEnum> DeleteDeelnemerAsync(Fietstocht fietstocht, Persoon persoon)
    {
        if (fietstocht == null) { throw new ApplicationException("parameter fietstocht can not be null"); }
        if (persoon == null) { throw new ApplicationException("parameter persoon can not be null"); }

        if (fietstocht.Deelnemers == null || fietstocht.Deelnemers?.FirstOrDefault(p => p.Id == persoon.Id) == null)
        {
            return ErrorCodeEnum.Ok;
        }

        fietstocht.Deelnemers.Remove(persoon);

        await _dbContext.SaveChangesAsync();
        
        return ErrorCodeEnum.Ok;
    }

    public async Task<FietstochtListItem[]?> GetAlleFietstochtListItems()
    {
        var list = _dbContext
            .Fietstochten?
            .Select(f => new FietstochtListItem { Id = f.Id, Titel = f.Titel })
            .ToArray();

        return await Task.FromResult(list);
    }

    public async Task SetFietstochten(Persoon persoon, List<FietstochtListItem> fietstochtListItems)
    {
        persoon.Fietstochten.Clear();
        foreach (var fietstochtListItem in fietstochtListItems)
        {
            var fietstocht = await GetById(fietstochtListItem.Id) ?? throw new ApplicationException($"Fietstocht with id {fietstochtListItem.Id} does not exist");
            persoon.Fietstochten.Add(fietstocht);
        }

        return;
    }
}
