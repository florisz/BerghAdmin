using BerghAdmin.DbContexts;

namespace BerghAdmin.Services.Facturen;

public class FactuurNummerService : IFactuurNummerService

{
    private readonly ApplicationDbContext _dbContext;
    private ILogger<FactuurNummerService> _logger;

    public FactuurNummerService(ApplicationDbContext dbContext, ILogger<FactuurNummerService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        logger.LogDebug($"FactuurNummerService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }

    public int GetNextNummer()
    {
        int nummer;
        
        // generate next consecutive invoice number based on last invoice number
        var lastInvoice = _dbContext.Facturen!.OrderByDescending(f => f.Nummer).FirstOrDefault();
        if (lastInvoice == null)
        {
            nummer = 1;
        }
        else
        {
            nummer = lastInvoice!.Nummer + 1;
        }

        return nummer;
    }

}
