using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Evenementen;

public class EvenementService : IEvenementService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;
    private readonly ILogger<EvenementService> _logger;

    public EvenementService(ApplicationDbContext context, IPersoonService persoonService, ILogger<EvenementService> logger)
    {
        _dbContext = context;
        _persoonService = persoonService;
        _logger = logger;
    }

    public Evenement? GetById(int id) 
        => _dbContext
            .Evenementen?
            .Include(ev => ev.BihzProject)
            .FirstOrDefault(e => e.Id== id);

    public Evenement? GetByTitel(string? titel) 
        => _dbContext
            .Evenementen?
            .Include(ev => ev.BihzProject)
            .FirstOrDefault(e => e.Titel == titel);

    public Evenement? GetByProjectId(int projectId)
    => _dbContext
            .Evenementen?
            .OfType<FietsTocht>()
            .Include(ev => ev.BihzProject)
            .Where(ev => ev.BihzProject != null)
            .FirstOrDefault(e => e.BihzProject!.Id == projectId);


    public async Task<ErrorCodeEnum> Save(Evenement evenement)
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

    public IEnumerable<T>? GetAll<T>()
        => _dbContext
            .Evenementen?
            .Include(e => e.Deelnemers)
            .OfType<T>();

    public IEnumerable<FietsTocht>? GetAllFietsTochten()
        => GetAll<FietsTocht>();

    public async Task<ErrorCodeEnum> AddDeelnemer(Evenement evenement, Persoon persoon)
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

        evenement.Deelnemers.Add (persoon);

        await _dbContext.SaveChangesAsync();

        _logger.LogInformation("Deelnemer added {deelnemer} to {evenement}", persoon.VolledigeNaam, evenement.Titel);

        return ErrorCodeEnum.Ok;
    }

    public async Task<ErrorCodeEnum> AddDeelnemer(Evenement evenement, int persoonId)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }

        var persoon = _persoonService.GetById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return await AddDeelnemer(evenement, persoon);
    }

    public async Task<ErrorCodeEnum> DeleteDeelnemer(Evenement evenement, Persoon persoon)
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

    public async Task<ErrorCodeEnum> DeleteDeelnemer(Evenement evenement, int persoonId)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }

        var persoon = _persoonService.GetById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return await DeleteDeelnemer(evenement, persoon);
    }

}
