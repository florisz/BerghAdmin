using System;
using System.Threading.Tasks;
using BerghAdmin.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services.Seeding
{
    public static class SeedHelper
    {
        public static bool DatabaseHasData(IRolService rolService)
            => rolService
                .GetRollen()
                .Any();

        public static async Task<Dictionary<RolTypeEnum, Rol>> InsertRollen(ApplicationDbContext dbContext)
        {
            var rolAmbassadeur = new Rol { Id = RolTypeEnum.Ambassadeur, Beschrijving = "Ambassadeur", MeervoudBeschrijving = "Ambassadeurs" };
            await dbContext.AddAsync(rolAmbassadeur);

            var rolBegeleider = new Rol { Id = RolTypeEnum.Begeleider, Beschrijving = "Begeleider", MeervoudBeschrijving = "Begeleiders" };
            await dbContext.AddAsync(rolBegeleider);

            var rolCommissieLid = new Rol { Id = RolTypeEnum.CommissieLid, Beschrijving = "Commissielid", MeervoudBeschrijving = "Commissieleden" };
            await dbContext.AddAsync(rolCommissieLid);

            var rolGolfer = new Rol { Id = RolTypeEnum.Golfer, Beschrijving = "Golfer", MeervoudBeschrijving = "Golfers" };
            await dbContext.AddAsync(rolGolfer);

            var rolMailingAbonnee = new Rol { Id = RolTypeEnum.MailingAbonnee, Beschrijving = "Mailing abonnee", MeervoudBeschrijving = "Mailing Abonnees" };
            await dbContext.AddAsync(rolMailingAbonnee);

            var rolFietser = new Rol { Id = RolTypeEnum.Fietser, Beschrijving = "Fietser", MeervoudBeschrijving = "Fieters" };
            await dbContext.AddAsync(rolFietser);

            var rolVriendVan = new Rol { Id = RolTypeEnum.VriendVan, Beschrijving = "Vriend van", MeervoudBeschrijving = "Vrienden van" };
            await dbContext.AddAsync(rolVriendVan);

            var rolVrijwilliger = new Rol { Id = RolTypeEnum.Vrijwilliger, Beschrijving = "Vrijwilliger", MeervoudBeschrijving = "Vrijwilligers" };
            await dbContext.AddAsync(rolVrijwilliger);

            await dbContext.SaveChangesAsync();

            var rollen = new Dictionary<RolTypeEnum, Rol>
            {
                { RolTypeEnum.Ambassadeur, rolAmbassadeur },
                { RolTypeEnum.Begeleider, rolBegeleider },
                { RolTypeEnum.CommissieLid, rolCommissieLid},
                { RolTypeEnum.Golfer, rolGolfer },
                { RolTypeEnum.MailingAbonnee, rolMailingAbonnee},
                { RolTypeEnum.Fietser, rolFietser },
                { RolTypeEnum.VriendVan, rolVriendVan },
                { RolTypeEnum.Vrijwilliger, rolVrijwilliger},
            };

            return rollen;
        }


    }
}
