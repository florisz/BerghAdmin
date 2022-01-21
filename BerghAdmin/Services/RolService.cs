using BerghAdmin.DbContexts;

namespace BerghAdmin.Services
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _dbContext;

        public RolService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Rol GetRolById(RolTypeEnum id)
        {
            var rol = _dbContext.Rollen?.SingleOrDefault(x => x.Id == id);
            
            if (rol == null)
            {
                throw new ApplicationException($"Rol with id {id} does not exist");
            }

            return rol;
        }

        public List<Rol> GetRollen()
        {
            if (_dbContext.Rollen == null)
            {
                return new List<Rol>();
            }

            return _dbContext.Rollen.ToList();
        }

    }
}
