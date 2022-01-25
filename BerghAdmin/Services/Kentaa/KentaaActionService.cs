using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.DbContexts;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public class KentaaActionService : IKentaaActionService
{
    private readonly ApplicationDbContext _dbContext;

    public KentaaActionService(ApplicationDbContext context)
    {
        _dbContext = context;
    }

    public void AddKentaaAction(KM.Action kentaaAction)
    {
        var action = GetByKentaaId(kentaaAction.Id);

        action = MapChanges(action, kentaaAction);

        Save(action);
    }

    public bool Exist(KentaaAction action)
        => GetByKentaaId(action.ActionId) != null;

    public IEnumerable<KentaaAction>? GetAll()
        => _dbContext
            .KentaaActions;

    public KentaaAction? GetById(int id)
       => _dbContext
            .KentaaActions?
            .SingleOrDefault(kd => kd.Id == id);

    public KentaaAction? GetByKentaaId(int kentaaId)
        => _dbContext
            .KentaaActions?
            .SingleOrDefault(ka => ka.ActionId == kentaaId);

    public ErrorCodeEnum Save(KentaaAction action)
    {
        try
        {
            if (action.Id == 0)
            {
                _dbContext
                    .KentaaActions?
                    .Add(action);
            }
            else
            {
                _dbContext
                    .KentaaActions?
                    .Update(action);
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

    private static KentaaAction MapChanges(KentaaAction? action, KM.Action kentaaAction)
    {
        if (action != null)
        {
            action.Update(kentaaAction);
        }
        else
        {
            action = new KentaaAction(kentaaAction);
        }

        return action;
    }
}
