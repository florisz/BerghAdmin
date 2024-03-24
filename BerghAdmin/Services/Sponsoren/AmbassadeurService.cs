using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow.ValueContentAnalysis;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Sponsoren;

public class AmbassadeurService : IAmbassadeurService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AmbassadeurService> _logger;
    
    public AmbassadeurService(ApplicationDbContext dbContext, ILogger<AmbassadeurService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        logger.LogDebug($"AmbassadeurService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }

    public Task<IEnumerable<Ambassadeur>?> GetAll()
    {
        return Task.FromResult((IEnumerable<Ambassadeur>?)_dbContext.Ambassadeurs?.Include(a => a.ContactPersoon1));
    }

    public Task<Ambassadeur?> GetById(int id)
    {
        return Task.FromResult(_dbContext
            .Ambassadeurs?
            .Include(a => a.ContactPersoon1)
            .FirstOrDefault(s => s.Id == id));
    }

    public Task<Ambassadeur?> GetByNaam(string naam)
    {
        _logger.LogDebug($"Get ambassadeur by naam {naam}; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");

        var ambassadeur = _dbContext
                .Ambassadeurs?
                .Include(a => a.ContactPersoon1)
                .SingleOrDefault(s => s.Naam == naam);

        _logger.LogDebug($"Sponsor (id={ambassadeur?.Id}) with naam {ambassadeur?.Naam} retrieved by naam {naam} was {((ambassadeur == null) ? "NOT Ok" : "Ok")}");

        return Task.FromResult(ambassadeur);
    }

    public async Task<ErrorCodeEnum> SaveAsync(Ambassadeur ambassadeur)
    {
        _logger.LogDebug($"Entering save ambassadeur {ambassadeur.Naam}");
        if (ambassadeur == null)
        {
            throw new ApplicationException("ambassadeur parameter can not be null");
        }

        if (ambassadeur.Id == 0)
        {
            if (await GetByNaam(ambassadeur.Naam) != null)
            {
                return ErrorCodeEnum.Conflict;
            }

            _dbContext.Ambassadeurs?.Add(ambassadeur);
        }
        else
        {
            _dbContext.Ambassadeurs?.Update(ambassadeur);
        }
        await _dbContext.SaveChangesAsync();

        return ErrorCodeEnum.Ok;
    }
}
