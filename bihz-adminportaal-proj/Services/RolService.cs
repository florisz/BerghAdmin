using BIHZ.AdminPortaal.Data;
using BIHZ.AdminPortaal.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BIHZ.AdminPortaal.Services
{
    public class RolService : IRolService
    {
        private readonly ApplicationDbContext _dbContext;

        public RolService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public Rol GetRolById(int id)
        {
            var rol = _dbContext.Rollen.SingleOrDefault(x => x.Id == id);
            
            return rol;
        }

        public List<Rol> GetRollen()
        {
            return _dbContext.Rollen.ToList();
        }

    }
}
