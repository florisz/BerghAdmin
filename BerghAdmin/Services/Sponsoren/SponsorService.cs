using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Sponsoren;

public class SponsorService : ISponsorService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;
    private readonly ILogger<SponsorService> _logger;
    
    public SponsorService(ApplicationDbContext dbContext, IPersoonService persoonService, ILogger<SponsorService> logger)
    {
        _dbContext = dbContext;
        _persoonService = persoonService;
        _logger = logger;
        logger.LogDebug($"SponsorService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }

    public IEnumerable<T>? GetAll<T>()
    {
        return (IEnumerable<T>?)_dbContext
            .Sponsoren;
    }

    public IEnumerable<Ambassadeur>? GetAllAmbassadeurs()
        => GetAll<Ambassadeur>();

    public IEnumerable<GolfdagSponsor>? GetAllGolfdagSponsoren()
        => GetAll<GolfdagSponsor>();

    public Sponsor? GetById(int id)
    {
        return _dbContext
            .Sponsoren?
            .FirstOrDefault(s => s.Id == id);
    }

    public Sponsor? GetByNaam(string naam)
    {
        _logger.LogDebug($"Get sponsor by naam {naam}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var sponsor = _dbContext
                .Sponsoren?
                .SingleOrDefault(s => s.Naam== naam);

        _logger.LogDebug($"Sponsor (id={sponsor?.Id}) with naam {sponsor?.Naam} retrieved by emailadres {naam} was {((sponsor == null) ? "NOT Ok" : "Ok")}");

        return sponsor;
    }

    public async Task<ErrorCodeEnum> SaveAsync(Sponsor sponsor)
    {
        _logger.LogDebug($"Entering save sponsor {sponsor.Naam}");
        if (sponsor == null)
        {
            throw new ApplicationException("Sponsor parameter can not be null");
        }

        if (sponsor.Id == 0)
        {
            if (GetByNaam(sponsor.Naam) != null)
            {
                return ErrorCodeEnum.Conflict;
            }

            _dbContext.Sponsoren?.Add(sponsor);
        }
        else
        {
            _dbContext.Sponsoren?.Update(sponsor);
        }
        await _dbContext.SaveChangesAsync();

        return ErrorCodeEnum.Ok;
    }
}
