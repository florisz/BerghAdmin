using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using System.Text.Json;

namespace BerghAdmin.Services.Kentaa;

public class KentaaActionService : IKentaaActionService
{
    private readonly ApplicationDbContext dbContext;
    private readonly ILogger<KentaaActionService> logger;

    public KentaaActionService(ApplicationDbContext context, ILogger<KentaaActionService> logger)
    {
        this.dbContext = context;
        this.logger = logger;
    }

    public void AddKentaaAction(KM.Action kentaaAction)
    {
        var action = GetByKentaaId(kentaaAction.Id);

        action = MapChanges(action, kentaaAction);

        logger.LogInformation("About to save action {action}", JsonSerializer.Serialize(action));
        Save(action);
    }

    public bool Exist(KentaaAction action)
        => GetByKentaaId(action.ActionId) != null;

    public IEnumerable<KentaaAction>? GetAll()
        => dbContext
            .KentaaActions;

    public KentaaAction? GetById(int id)
       => dbContext
            .KentaaActions?
            .SingleOrDefault(kd => kd.Id == id);

    public KentaaAction? GetByKentaaId(int kentaaId)
        => dbContext
            .KentaaActions?
            .SingleOrDefault(ka => ka.ActionId == kentaaId);

    public ErrorCodeEnum Save(KentaaAction action)
    {
        try
        {
            if (action.Id == 0)
            {
                dbContext
                    .KentaaActions?
                    .Add(action);
            }
            else
            {
                dbContext
                    .KentaaActions?
                    .Update(action);
            }

            dbContext.SaveChanges();
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
