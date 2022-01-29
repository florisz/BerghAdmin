using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.Services.Kentaa;

public class KentaaUserService : IKentaaUserService
{
    private readonly ApplicationDbContext _dbContext;

    public KentaaUserService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void AddKentaaUser(KM.User user)
    {
        var bihzUser = GetByKentaaId(user.Id);

        bihzUser = MapChanges(bihzUser, user);

        Save(bihzUser);
    }

    public void AddKentaaUsers(IEnumerable<KM.User> users)
    {
        foreach (var user in users)
        {
            AddKentaaUser(user);
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

    private BihzUser MapChanges(BihzUser? bihzUser, KM.User user)
    {
        if (bihzUser != null)
        {
            bihzUser.Map(user);
        }
        else
        {
            bihzUser = new BihzUser(user);
        }

        return bihzUser;
    }
}
