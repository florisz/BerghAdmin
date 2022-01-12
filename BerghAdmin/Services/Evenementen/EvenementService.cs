using BerghAdmin.DbContexts;
using BerghAdmin.General;
using static System.Net.WebRequestMethods;

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


    public ErrorCodeEnum SaveEvenement(Evenement evenement)
    {
        if (evenement.Id == 0)
        {
            if (GetByName(evenement.Naam) != null)
            {
                return ErrorCodeEnum.Conflict;
            }

            _dbContext.Evenementen?.Add(evenement);
        }
        else
        {
            _dbContext.Evenementen?.Update(evenement);
        }
        _dbContext.SaveChanges();

        return ErrorCodeEnum.Ok;
    }

    public IEnumerable<T>? GetAllEvenementen<T>()
    {
        return _dbContext
                    .Evenementen?
                    .OfType<T>();
    }

}
