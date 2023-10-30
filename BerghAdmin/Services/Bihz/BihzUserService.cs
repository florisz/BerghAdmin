using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;
using BerghAdmin.General;

using KM = BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Services.Bihz;

public class BihzUserService : IBihzUserService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IPersoonService _persoonService;
    private readonly ILogger<BihzUserService> _logger;

    public BihzUserService(ApplicationDbContext context, IPersoonService persoonService, ILogger<BihzUserService> logger)
    {
        _dbContext = context;
        _persoonService = persoonService;
        _logger = logger;
    }

    public void Add(BihzUser user)
    {
        _logger.LogDebug("Add BihzUser with KentaaId {UserId}", user.UserId);

        var bihzUser = MapChanges(GetByKentaaId(user.UserId), user);

        if (bihzUser.PersoonId == null)
        {
            // Persoon has not been linked to a registered Kentaa user yet,
            // link thru the email address of the Kentaa user
            var persoon = _persoonService.GetByEmailAdres(bihzUser.Email ?? "no-email");
            if (persoon == null)
            {
                _logger.LogError("Kentaa user with id {UserId} can not be processed; reason: the corresponding persoon with email address {Email} is unknown.",
                        bihzUser.UserId, bihzUser.Email);
                return;
            }
            persoon.BihzUser = bihzUser;
            bihzUser.PersoonId = persoon.Id;

            _persoonService.SavePersoonAsync(persoon);
        }

        Save(bihzUser);
        _logger.LogInformation("Kentaa user with id {UserId} successfully linked to persoon with id {PersoonId}", bihzUser.UserId, bihzUser.PersoonId);
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

    private BihzUser MapChanges(BihzUser? currentUser, BihzUser user)
    {
        if (currentUser == null)
            return new BihzUser(user);

        return currentUser.UpdateFrom(user);
    }

}
