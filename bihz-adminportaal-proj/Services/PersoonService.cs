using BIHZ.AdminPortaal.Data;
using BIHZ.AdminPortaal.DbContexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BIHZ.AdminPortaal.Services
{
    public class PersoonService : IPersoonService
    {
        private readonly ApplicationDbContext _dbContext;

        public PersoonService(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public void DeletePersoon(int id)
        {
            var persoon = _dbContext.Personen.FirstOrDefault(x => x.Id == id);
            if(persoon != null)
            {
                _dbContext.Personen.Remove(persoon);
                _dbContext.SaveChanges();
            }
        }

        public Persoon GetPersoonById(int id)
        {
            var persoon = _dbContext.Personen
                            .Include(p => p.Rollen)
                            .SingleOrDefault(x => x.Id == id);
            
            return persoon;
        }

        public List<Persoon> GetPersonen()
        {
            return _dbContext.Personen
                    .Include(p => p.Rollen)
                    .ToList();
        }

        public void SavePersoon(Persoon persoon)
        {
            if (persoon.Id == 0) 
            {
                _dbContext.Personen.Add(persoon);
            }
            else
            { 
                _dbContext.Personen.Update(persoon);
            }
            _dbContext.SaveChanges();
        }
    }
}
