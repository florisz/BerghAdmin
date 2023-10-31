using BerghAdmin.Data;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Evenementen;

public class EvenementService : IEvenementService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;
    private readonly ILogger<EvenementService> _logger;

    public EvenementService(ApplicationDbContext dbContext, IPersoonService persoonService, ILogger<EvenementService> logger)
    {
        _dbContext = dbContext;
        _persoonService = persoonService;
        _logger = logger;
        logger.LogDebug($"EvenementService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
}

    public Evenement? GetById(int id)
    {
        return _dbContext
            .Evenementen?
            .FirstOrDefault(e => e.Id == id);
    }

    public Evenement? GetByTitel(string? titel)
    {
        return _dbContext
            .Evenementen?
            .FirstOrDefault(e => e.Titel == titel);
    }

    public Evenement? GetByProjectId(int projectId)
    {
        return _dbContext
            .Evenementen?
            .OfType<FietsTocht>()
            .FirstOrDefault(e => e.KentaaProjectId == projectId);
    }
    public Evenement? GetByProject(BihzProject project)
    {
        if (project == null)
        {
            return null;
        }

        var evenement = GetByProjectId(project.ProjectId);
        if (evenement == null)
        {
            evenement = GetByTitel(project.Titel);
        }

        return evenement;
    }

    public IEnumerable<T>? GetAll<T>()
    {
        return _dbContext
            .Evenementen?
            .Include(e => e.Deelnemers)
            .OfType<T>();
    }
    public IEnumerable<FietsTocht>? GetAllFietsTochten()
        => GetAll<FietsTocht>();

    public async Task<ErrorCodeEnum> SaveAsync(Evenement evenement)
    {
        _logger.LogDebug("Entering save evenement {evenement}", evenement.Titel);
        if (evenement == null)
        {
            throw new ApplicationException("Evenement parameter can not be null");
        }

        if (evenement.Id == 0)
        {
            if (GetByTitel(evenement.Titel) != null)
            {
                return ErrorCodeEnum.Conflict;
            }

            _dbContext.Evenementen?.Add(evenement);
        }
        else
        {
            _dbContext.Evenementen?.Update(evenement);
        }
        await _dbContext.SaveChangesAsync();

        return ErrorCodeEnum.Ok;
    }

    public async Task<ErrorCodeEnum> AddDeelnemerAsync(Evenement evenement, Persoon persoon)
    {
        _logger.LogDebug("Entering Add deelnemer {persoon} to {evenement}",
            persoon.VolledigeNaam, evenement.Titel);

        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }
        if (persoon == null) { throw new ApplicationException("parameter persoon can not be null"); }

        if (evenement.Deelnemers?.FirstOrDefault(p => p.Id == persoon.Id) != null)
        {
            return ErrorCodeEnum.Conflict;
        }

        if (evenement.Deelnemers == null)
        {
            evenement.Deelnemers = new HashSet<Persoon>();
        }

        evenement.Deelnemers.Add(persoon);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Deelnemer added {deelnemer} to {evenement}", persoon.VolledigeNaam, evenement.Titel);
        return ErrorCodeEnum.Ok;
    }

    public async Task<ErrorCodeEnum> AddDeelnemerAsync(Evenement evenement, int persoonId)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }

        var persoon = _persoonService.GetById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return await AddDeelnemerAsync(evenement, persoon);
    }

    public async Task<ErrorCodeEnum> DeleteDeelnemerAsync(Evenement evenement, Persoon persoon)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }
        if (persoon == null) { throw new ApplicationException("parameter persoon can not be null"); }

        if (evenement.Deelnemers == null || evenement.Deelnemers?.FirstOrDefault(p => p.Id == persoon.Id) == null)
        {
            return ErrorCodeEnum.Ok;
        }

        evenement.Deelnemers.Remove(persoon);

        await _dbContext.SaveChangesAsync();
        
        return ErrorCodeEnum.Ok;
    }

    public async Task<ErrorCodeEnum> DeleteDeelnemerAsync(Evenement evenement, int persoonId)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }

        var persoon = _persoonService.GetById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return await DeleteDeelnemerAsync(evenement, persoon);
    }

}
