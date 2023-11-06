using BerghAdmin.DbContexts;
using BerghAdmin.General;
using BerghAdmin.Services.Betalingen;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace BerghAdmin.Services
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _dbContext;
        private ILogger<RolService> _logger;

        public RolService(ApplicationDbContext dbContext, ILogger<RolService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
            logger.LogDebug($"RolService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={dbContext.ContextId}");
        }

        // code is only used once during seeding so it does not beed to be rock solid
        public async Task AddRol(Rol rol)
        {
            _logger.LogDebug($"Entering add rol {rol.Beschrijving}");
            _dbContext.Rollen?.Add(rol);
            await _dbContext.SaveChangesAsync();

            return;
        }

        public Rol GetRolById(RolTypeEnum id)
        {
            var rol = _dbContext
                .Rollen?
                .SingleOrDefault(x => x.Id == Convert.ToInt32(id));
            
            if (rol == null)
            {
                throw new ApplicationException($"Rol with id {id} does not exist");
            }

            return rol;
        }

        public List<Rol> GetRollen()
        {
            var rollen = _dbContext
                    .Rollen;

            if (rollen == null)
            {
                return new List<Rol>();
            }

            return rollen.ToList();
        }

    }
}
