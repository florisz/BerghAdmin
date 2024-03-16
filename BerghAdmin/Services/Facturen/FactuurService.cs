using BerghAdmin.DbContexts;
using BerghAdmin.Services.DateTimeProvider;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Facturen;

public class FactuurService : IFactuurService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IDateTimeProvider _dateTimeProvider;
    private ILogger<FactuurService> _logger;

    public FactuurService(ApplicationDbContext dbContext, IDateTimeProvider dateTimeProvider, ILogger<FactuurService> logger)
    {
        _dbContext = dbContext;
        _dateTimeProvider = dateTimeProvider;
        _logger = logger;
        logger.LogDebug($"FactuurService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }

    public Task<List<Factuur>> GetFacturenAsync(int jaar)
        => _dbContext.Facturen!
            .Where(f => f.Datum.Year == jaar)
            .OrderBy(f => f.Nummer)
            .ToListAsync();


    public async Task<Factuur?> GetFactuurAsync(int nummer)
        => await _dbContext
                    .Facturen!
                    .FirstOrDefaultAsync(f => f.Nummer == nummer);



    public async Task<Factuur?> GetNewFactuurAsync()
        => await GetNewFactuurAsync(_dateTimeProvider.Now);

    public async Task<Factuur?> GetNewFactuurAsync(DateTime dateTime)
        => await GetNewFactuurAsync(GetNextFactuurNummer(), dateTime);

    public async Task<Factuur?> GetNewFactuurAsync(int nummer, DateTime dateTime)
    {
        int tries = 0;
        bool returnValue = false;
        Factuur factuur = null!;

        while (!returnValue)
        {
            factuur = new Factuur(nummer, dateTime);
            returnValue = await SaveFactuurAsync(factuur);

            if (!returnValue && tries++ > 3)
            {
                throw new ApplicationException("Could not generate new factuur, tried it 3 times");
            }
        }

        return factuur;
    }

    public async Task<bool> SaveFactuurAsync(Factuur factuur)
    {
        if (FactuurExists(factuur.Nummer))
        {
            return false;
        }
        _dbContext.Facturen!.Add(factuur);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private int GetNextFactuurNummer()
    {
        int nummer;

        // generate next consecutive invoice number based on last invoice number
        var laatsteFactuur = _dbContext.Facturen!.OrderByDescending(f => f.Nummer).FirstOrDefault();
        if (laatsteFactuur == null)
        {
            nummer = 1;
        }
        else
        {
            nummer = laatsteFactuur!.Nummer + 1;
        }

        return nummer;
    }

    private bool FactuurExists(int nummer)
        => _dbContext.Facturen!.Any(f => f.Nummer == nummer);

}
