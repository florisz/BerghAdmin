using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.DbContexts;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public class KentaaUserService : IKentaaUserService
{
    private readonly ApplicationDbContext _dbContext;

    public KentaaUserService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void AddKentaaUser(KM.User kentaaUser)
    {
        var user = GetByKentaaId(kentaaUser.Id);

        user = MapChanges(user, kentaaUser);

        Save(user);
    }

    public void AddKentaaUsers(IEnumerable<KM.User> kentaaUsers)
    {
        foreach (var kentaaUser in kentaaUsers)
        {
            AddKentaaUser(kentaaUser);
        }
    }

    public bool Exist(KentaaUser user)
        => GetByKentaaId(user.Id) != null;

    public IEnumerable<KentaaUser>? GetAll()
        => _dbContext
            .KentaaUsers;

    public KentaaUser? GetById(int id)
       => _dbContext
            .KentaaUsers?
            .SingleOrDefault(kd => kd.Id == id);

    public KentaaUser? GetByKentaaId(int kentaaId)
        => _dbContext
            .KentaaUsers?
            .SingleOrDefault(ka => ka.UserId == kentaaId);

    public ErrorCodeEnum Save(KentaaUser user)
    {
        try
        {
            if (user.Id == 0)
            {
                _dbContext
                    .KentaaUsers?
                    .Add(user);
            }
            else
            {
                _dbContext
                    .KentaaUsers?
                    .Update(user);
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

    private KentaaUser MapChanges(KentaaUser? user, KM.User kentaaUser)
    {
        if (user != null)
        {
            user.Update(kentaaUser);
        }
        else
        {
            user = new KentaaUser(kentaaUser);
        }

        return user;
    }
}
