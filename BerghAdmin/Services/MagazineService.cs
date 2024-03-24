using BerghAdmin.DbContexts;

namespace BerghAdmin.Services;

public class MagazineService : IMagazineService
{
    private readonly ApplicationDbContext _dbContext;
    private ILogger<RolService> _logger;

    public MagazineService(ApplicationDbContext dbContext, ILogger<RolService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        logger.LogDebug($"MagazineService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
    }

    // code is only used once during seeding so it does not beed to be rock solid
    public async Task AddMagazine(MagazineJaar magazine)
    {
        _logger.LogDebug($"Entering add magazine {magazine.Jaar}");
        _dbContext.MagazineJaren?.Add(magazine);
        await _dbContext.SaveChangesAsync();

        return;
    }

    public async Task DeleteAll()
    {
        _dbContext.MagazineJaren!.RemoveRange(GetMagazines());
        await _dbContext.SaveChangesAsync();
    }

    public MagazineJaar? GetMagazineById(int id) =>
        _dbContext
            .MagazineJaren?
            .SingleOrDefault(mj => mj.Id == id);

    public MagazineJaar? GetMagazineByJaar(string jaar) =>
        _dbContext
            .MagazineJaren?
            .SingleOrDefault(mj => mj.Jaar == jaar);

    public List<MagazineJaar> GetMagazines()
    {
        var magazines = _dbContext
                .MagazineJaren;

        if (magazines == null)
        {
            return new List<MagazineJaar>();
        }

        return magazines.ToList();
    }   

}
