﻿using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;

using System.Text.Json;

namespace BerghAdmin.Services.Bihz;

public class BihzActieService : IBihzActieService
{
    private readonly ApplicationDbContext dbContext;
    private readonly ILogger<BihzActieService> logger;
    private readonly IPersoonService _persoonService;

    public BihzActieService(ApplicationDbContext context, IPersoonService persoonService, ILogger<BihzActieService> logger)
    {
        this.dbContext = context;
        this._persoonService = persoonService;
        this.logger = logger;
    }

    public void Add(BihzActie action)
    {
        var bihzActie = GetByKentaaId(action.Id);

        //bihzActie = MapChanges(bihzActie, action);

        logger.LogInformation("About to save bihzActie {action}", JsonSerializer.Serialize(bihzActie));

        if (bihzActie.PersoonId == null)
        {
            LinkActieToPersoon(bihzActie);
        }

        Save(bihzActie);
    }

    public void Add(IEnumerable<BihzActie> actions)
    {
        foreach (var action in actions)
        {
            Add(action);
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


    private void LinkActieToPersoon(BihzActie bihzActie)
    {
        // link with kentaa user id does not exist yet; try email
        var persoon = _persoonService.GetByEmailAdres(bihzActie.Email ?? "no-email");

        if (persoon == null)
        {
            // TO BE DONE
            // report to admin "kentaa action can not be mapped"
        }
        if (persoon != null)
        {
            persoon.BihzActie = bihzActie;
            bihzActie.PersoonId = persoon.Id;
            _persoonService.SavePersoon(persoon);
        }
    }
}
