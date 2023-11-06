using BerghAdmin.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Betalingen;

public class EFBetalingenRepository : IBetalingenRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EFBetalingenRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(Betaling betaling)
    {
        _dbContext
            .Betalingen?
            .Add(betaling);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Betaling betaling)
    {
        _dbContext
            .Betalingen?
            .Update(betaling);
        await _dbContext.SaveChangesAsync();
    }

    public Betaling? GetByVolgnummer(string volgNummer)
    {
        return _dbContext
            .Betalingen?
            .FirstOrDefault(b => b.Volgnummer == volgNummer);
    }
    public IEnumerable<Betaling>? GetAll()
    {
        return _dbContext
            .Betalingen?
            .ToList();
    }
}
