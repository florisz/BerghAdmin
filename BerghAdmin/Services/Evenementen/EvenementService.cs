using BerghAdmin.DbContexts;

namespace BerghAdmin.Services.Evenementen;

public class EvenementService : IEvenementService
{
    private readonly ApplicationDbContext _dbContext;

    public EvenementService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public Evenement GetById(int id)
    {
        var evenement = _dbContext
                    .Evenementen
                    .Find(id);

        return evenement;
    }

    public Evenement GetByName(string name)
    {
        var evenement = _dbContext
                    .Evenementen
                    .FirstOrDefault(e => e.Naam == name);

        return evenement;
    }


    public void SaveEvenement(Evenement evenement)
    {
        if (evenement.Id == 0)
        {
            _dbContext.Evenementen?.Add(evenement);
        }
        else
        {
            _dbContext.Evenementen?.Update(evenement);
        }
        _dbContext.SaveChanges();
    }

}
