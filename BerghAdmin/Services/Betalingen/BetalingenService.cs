using BerghAdmin.DbContexts;

namespace BerghAdmin.Services.Betalingen;

public class BetalingenService : IBetalingenService
{
    private readonly IBetalingenRepository repo;
    private readonly ILogger<BetalingenService> _logger;

    public BetalingenService(IBetalingenRepository repo, ILogger<BetalingenService> logger)
    {
        this.repo = repo;
        _logger = logger;
    }

    public void Save(Betaling betaling)
    {
        _logger.LogDebug("Save betaling with volg nummer: {Volgnummer}", betaling.Volgnummer);

        if (betaling.Id == 0)
        {
            repo.Add(betaling);
            _logger.LogInformation("Betaling {Betaling} is added", betaling);
        }
        else
        {
            repo.Update(betaling);
            _logger.LogInformation("Betaling {Betaling} is updated", betaling);
        }
    }

    public Betaling? GetByVolgnummer(string volgNummer)
        => repo.GetByVolgnummer(volgNummer);

    public IEnumerable<Betaling>? GetAll()
        => repo.GetAll();
}
