﻿using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.DbContexts;
using BerghAdmin.General;
using System.Text.Json;
using BerghAdmin.Data.Kentaa;

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

    public void AddKentaaAction(KM.Action action)
    {
        var bihzActie = GetByKentaaId(action.Id);

        bihzActie = MapChanges(bihzActie, action);

        logger.LogInformation("About to save bihzActie {action}", JsonSerializer.Serialize(bihzActie));
        Save(bihzActie);
    }

    public void AddKentaaActions(IEnumerable<KM.Action> actions)
    {
        foreach (var action in actions)
        {
            AddKentaaAction(action);
        }
    }

    public bool Exist(BihzActie bihzActie)
        => GetByKentaaId(bihzActie.ActionId) != null;

    public IEnumerable<BihzActie>? GetAll()
        => dbContext
            .BihzActies;

    public BihzActie? GetById(int id)
       => dbContext
            .BihzActies?
            .SingleOrDefault(kd => kd.Id == id);

    public BihzActie? GetByKentaaId(int kentaaId)
        => dbContext
            .BihzActies?
            .SingleOrDefault(ka => ka.ActionId == kentaaId);

    public ErrorCodeEnum Save(BihzActie bihzActie)
    {
        try
        {
            if (bihzActie.Id == 0)
            {
                dbContext
                    .BihzActies?
                    .Add(bihzActie);
            }
            else
            {
                dbContext
                    .BihzActies?
                    .Update(bihzActie);
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

    private static BihzActie MapChanges(BihzActie? bihzActie, KM.Action kentaaAction)
    {
        if (bihzActie != null)
        {
            bihzActie.Map(kentaaAction);
        }
        else
        {
            bihzActie = new BihzActie(kentaaAction);
        }

        return bihzActie;
    }
}
