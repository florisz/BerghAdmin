using BerghAdmin.Data.Kentaa;
using BerghAdmin.DbContexts;

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

    public async Task AddAsync(BihzUser user)
    {
        _logger.LogDebug("AddAsync BihzUser with KentaaId {UserId}", user.UserId);

        var bihzUser = MapChanges(await GetByKentaaId(user.UserId), user);

        if (bihzUser.PersoonId == null)
        {
            // Persoon has not been linked to a registered Kentaa user yet,
            // link thru the email address of the Kentaa user
            var persoon = await _persoonService.GetByEmailAdres(bihzUser.Email ?? "no-email");
            if (persoon == null)
            {
                _logger.LogError("Kentaa user with id {UserId} can not be processed; reason: the corresponding persoon with email address {Email} is unknown.",
                        bihzUser.UserId, bihzUser.Email);
                return;
            }
            persoon.BihzUser = bihzUser;
            bihzUser.PersoonId = persoon.Id;

            await _persoonService.SavePersoonAsync(persoon);
        }

        await SaveAsync(bihzUser);
        _logger.LogInformation("Kentaa user with id {UserId} successfully linked to persoon with id {PersoonId}", bihzUser.UserId, bihzUser.PersoonId);
    }

    public async Task AddAsync(IEnumerable<BihzUser> users)
    {
        foreach (var user in users)
        {
            await AddAsync(user);
        }
    }

    public async Task<bool> ExistAsync(BihzUser bihzUser)
        => await GetByKentaaId(bihzUser.Id) != null;

    public Task<List<BihzUser>> GetAll()
    { 
        var bihzUsers = _dbContext
                            .BihzUsers?
                            .ToList();

        return Task.FromResult(bihzUsers ?? new List<BihzUser>());
    }

    public Task<BihzUser?> GetById(int id)
       => Task.FromResult(_dbContext
            .BihzUsers?
            .SingleOrDefault(kd => kd.Id == id));

    public Task<BihzUser?> GetByKentaaId(int kentaaId)
        => Task.FromResult(_dbContext
            .BihzUsers?
            .SingleOrDefault(ka => ka.UserId == kentaaId));

    public async Task SaveAsync(BihzUser bihzUser)
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

        await _dbContext.SaveChangesAsync();
    }

    private BihzUser MapChanges(BihzUser? currentUser, BihzUser user)
    {
        if (currentUser == null)
            return new BihzUser(user);

        return currentUser.UpdateFrom(user);
    }

}
