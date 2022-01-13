using BerghAdmin.DbContexts;
using BerghAdmin.General;
using static System.Net.WebRequestMethods;

namespace BerghAdmin.Services.Evenementen;

public class EvenementService : IEvenementService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;

    public EvenementService(ApplicationDbContext context, IPersoonService persoonService)
    {
        _dbContext = context;
        _persoonService = persoonService;
    }

    public Evenement? GetById(int id)
    {
        var evenement = _dbContext
                    .Evenementen?
                    .Find(id);

        return evenement;
    }

    public Evenement? GetByName(string name)
    {
        var evenement = _dbContext
                    .Evenementen?
                    .FirstOrDefault(e => e.Naam == name);

        return evenement;
    }


    public ErrorCodeEnum SaveEvenement(Evenement evenement)
    {
        if (evenement == null) { throw new ApplicationException("Evenement parameter can never be null"); }

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

    public ErrorCodeEnum AddDeelnemer(Evenement evenement, Persoon persoon)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }
        if (persoon == null) { throw new ApplicationException("parameter persoon can not be null"); }

        if (evenement.Deelnemers?.FirstOrDefault(p => p.Id == persoon.Id) != null)
        { 
            return ErrorCodeEnum.Conflict; 
        }

        if (evenement.Deelnemers == null)
        {
            evenement.Deelnemers = new HashSet<Persoon>();
        }

        evenement.Deelnemers.Add (persoon);

        _dbContext.SaveChanges();

        return ErrorCodeEnum.Ok;
    }

    public ErrorCodeEnum AddDeelnemer(Evenement evenement, int persoonId)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }

        var persoon = _persoonService.GetPersoonById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return AddDeelnemer (evenement, persoon);
    }

    public ErrorCodeEnum DeleteDeelnemer(Evenement evenement, Persoon persoon)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }
        if (persoon == null) { throw new ApplicationException("parameter persoon can not be null"); }

        if (evenement.Deelnemers == null || evenement.Deelnemers?.FirstOrDefault(p => p.Id == persoon.Id) == null)
        {
            return ErrorCodeEnum.Ok;
        }

        evenement.Deelnemers.Remove(persoon);

        _dbContext.SaveChanges();

        return ErrorCodeEnum.Ok;
    }

    public ErrorCodeEnum DeleteDeelnemer(Evenement evenement, int persoonId)
    {
        if (evenement == null) { throw new ApplicationException("parameter evenement can not be null"); }

        var persoon = _persoonService.GetPersoonById(persoonId);
        if (persoon == null)
        {
            return ErrorCodeEnum.NotFound;
        }

        return DeleteDeelnemer(evenement, persoon);
    }
}
