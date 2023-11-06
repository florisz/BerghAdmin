using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Evenementen;

public class FietstochtenService : IFietstochtenService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;
    private readonly ILogger<FietstochtenService> _logger;

    public FietstochtenService(ApplicationDbContext dbContext, IPersoonService persoonService, ILogger<FietstochtenService> logger)
    {
        _dbContext = dbContext;
        _persoonService = persoonService;
        _logger = logger;
        logger.LogDebug($"FietstochtenService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
}

    public IEnumerable<Fietstocht>? GetAll()
    {
        return _dbContext
            .Fietstochten?
            .Include(e => e.Deelnemers);
    }
    public Fietstocht? GetById(int id)
    {
        return _dbContext
            .Fietstochten?
            .FirstOrDefault(e => e.Id == id);
    }

    public Fietstocht? GetByTitel(string? titel)
    {
        return _dbContext
            .Fietstochten?
            .FirstOrDefault(e => e.Titel == titel);
    }

    public Fietstocht? GetByProjectId(int projectId)
    {
        return _dbContext
            .Fietstochten?
            .FirstOrDefault(e => e.KentaaProjectId == projectId);
    }
    public Fietstocht? GetByProject(BihzProject project)
    {
        if (project == null)
        {
            return null;
        }

        var fietstocht = GetByProjectId(project.ProjectId);
        if (fietstocht == null)
        {
            fietstocht = GetByTitel(project.Titel);
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
            if (GetByTitel(fietstocht.Titel) != null)
            {
                return ErrorCodeEnum.Conflict;
            }

            _dbContext.Fietstochten?.Add(fietstocht);
        }
        else
        {
            _dbContext.Fietstochten?.Update(fietstocht);
        }
        await _dbContext.SaveChangesAsync();

        return ErrorCodeEnum.Ok;
    }

    public async Task<ErrorCodeEnum> AddDeelnemerAsync(Fietstocht fietstocht, Persoon persoon)
    {
        _logger.LogDebug($"Entering Add deelnemer {persoon.VolledigeNaam} to {fietstocht.Titel}");

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

    public async Task<ErrorCodeEnum> AddDeelnemerAsync(Fietstocht fietstocht, int persoonId)
    {
        if (fietstocht == null) { throw new ApplicationException("parameter fietstocht can not be null"); }

        var persoon = _persoonService.GetById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return await AddDeelnemerAsync(fietstocht, persoon);
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

    public async Task<ErrorCodeEnum> DeleteDeelnemerAsync(Fietstocht fietstocht, int persoonId)
    {
        if (fietstocht == null) { throw new ApplicationException("parameter fietstocht can not be null"); }

        var persoon = _persoonService.GetById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return await DeleteDeelnemerAsync(fietstocht, persoon);
    }

}
