using BerghAdmin.DbContexts;

namespace BerghAdmin.Services.Betalingen;

public class EFBetalingenRepository : IBetalingenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EFBetalingenRepository(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void Add(Betaling betaling)
    {
        _dbContext
            .Betalingen?
            .Add(betaling);
        _dbContext.SaveChanges();
    }

    public void Update(Betaling betaling)
    {
        _dbContext
            .Betalingen?
            .Update(betaling);
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
