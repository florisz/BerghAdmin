using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;

using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Services.Bihz;

public class BihzUserService : IBihzUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;

    public BihzUserService(ApplicationDbContext context, IPersoonService persoonService)
    {
        _dbContext = context;
        _persoonService = persoonService;
    }

    public void Add(BihzUser user)
    {
        var currentUser = GetByKentaaId(user.UserId);

        currentUser = MapChanges(currentUser, user);

        if (currentUser.PersoonId == null)
        {
            LinkUserToPersoon(currentUser);
        }

        Save(currentUser);
    }

    public void Add(IEnumerable<BihzUser> users)
    {
        foreach (var user in users)
        {
            Add(user);
        }
    }

    public bool Exist(BihzUser bihzUser)
        => GetByKentaaId(bihzUser.Id) != null;

    public IEnumerable<BihzUser>? GetAll()
        => _dbContext
            .BihzUsers;

    public BihzUser? GetById(int id)
       => _dbContext
            .BihzUsers?
            .SingleOrDefault(kd => kd.Id == id);

    public BihzUser? GetByKentaaId(int kentaaId)
        => _dbContext
            .BihzUsers?
            .SingleOrDefault(ka => ka.UserId == kentaaId);

    public ErrorCodeEnum Save(BihzUser bihzUser)
    {
        try
        {
            if (bihzUser.Id == 0)
            {
                _dbContext
                    .BihzUsers?
                    .Add(bihzUser);
            }
            else
            {
                _dbContext
                    .BihzUsers?
                    .Update(bihzUser);
            }

            _dbContext.SaveChanges();
        }
        catch (Exception)
        {
            // log exception
            return ErrorCodeEnum.Conflict;
        }

        return ErrorCodeEnum.Ok;
    }

    private void LinkUserToPersoon(BihzUser bihzUser)
    {
        // link with email address
        var persoon = _persoonService.GetByEmailAdres(bihzUser.Email ?? "no-email");

        if (persoon == null)
        {
            // TO BE DONE
            // report to admin "kentaa user can not be mapped"
        }
        if (persoon != null)
        {
            persoon.BihzUser = bihzUser;
            bihzUser.PersoonId = persoon.Id;
            _persoonService.SavePersoon(persoon);
        }
    }

    private BihzUser MapChanges(BihzUser? currentUser, BihzUser user)
    {
        if (currentUser == null)
            return new BihzUser(user);

        return currentUser.UpdateFrom(user);
    }

}
