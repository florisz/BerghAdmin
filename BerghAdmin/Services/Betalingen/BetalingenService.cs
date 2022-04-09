using BerghAdmin.DbContexts;

namespace BerghAdmin.Services.Betalingen;

public class BetalingenService : IBetalingenService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<BetalingenService> _logger;

    public BetalingenService(ApplicationDbContext context, ILogger<BetalingenService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }

    public void Save(Betaling betaling)
    {
        _logger.LogDebug("Save betaling with volg nummer: {Volgnummer}", betaling.Volgnummer);

        if (betaling.Id == 0)
        {
            _dbContext
                .Betalingen?
                .Add(betaling);
            _logger.LogInformation("Betaling {Betaling} is added", betaling);
        }
        else
        {
            _dbContext
                .Betalingen?
                .Update(betaling);
            _logger.LogInformation("Betaling {Betaling} is updated", betaling);
        }
        _dbContext.SaveChanges();
    }

    public Betaling? GetByVolgnummer(string volgNummer)
        => _dbContext
            .Betalingen?
            .FirstOrDefault(b => b.Volgnummer == volgNummer);

    public IEnumerable<Betaling>? GetAll()
        => _dbContext
            .Betalingen?
            .ToList();
}
