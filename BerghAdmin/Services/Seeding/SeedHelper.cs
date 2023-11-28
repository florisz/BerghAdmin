using BerghAdmin.DbContexts;

namespace BerghAdmin.Services.Seeding
{
    public static class SeedHelper
    {
        public static bool DatabaseHasData(IRolService rolService)
            => rolService
                .GetRollen()
                .Any();

        public static async Task<Dictionary<RolTypeEnum, Rol>> InsertRollen(IRolService rolService)
        {
            var rolContactpersoon = new Rol { Id = Convert.ToInt32(RolTypeEnum.Contactpersoon), Beschrijving = "Contactpersoon", MeervoudBeschrijving = "Contactpersonen" };
            await rolService.AddRol(rolContactpersoon);

            var rolBegeleider = new Rol { Id = Convert.ToInt32(RolTypeEnum.Begeleider), Beschrijving = "Begeleider", MeervoudBeschrijving = "Begeleiders" };
            await rolService.AddRol(rolBegeleider);

            var rolCommissieLid = new Rol { Id = Convert.ToInt32(RolTypeEnum.CommissieLid), Beschrijving = "Commissielid", MeervoudBeschrijving = "Commissieleden" };
            await rolService.AddRol(rolCommissieLid);

            var rolGolfer = new Rol { Id = Convert.ToInt32(RolTypeEnum.Golfer), Beschrijving = "Golfer", MeervoudBeschrijving = "Golfers" };
            await rolService.AddRol(rolGolfer);

            var rolMailingAbonnee = new Rol { Id = Convert.ToInt32(RolTypeEnum.MailingAbonnee), Beschrijving = "Mailing abonnee", MeervoudBeschrijving = "Mailing Abonnees" };
            await rolService.AddRol(rolMailingAbonnee);

            var rolFietser = new Rol { Id = Convert.ToInt32(RolTypeEnum.Fietser), Beschrijving = "Fietser", MeervoudBeschrijving = "Fieters" };
            await rolService.AddRol(rolFietser);

            var rolVriendVan = new Rol { Id = Convert.ToInt32(RolTypeEnum.VriendVan), Beschrijving = "Vriend van", MeervoudBeschrijving = "Vrienden van" };
            await rolService.AddRol(rolVriendVan);

            var rolVrijwilliger = new Rol { Id = Convert.ToInt32(RolTypeEnum.Vrijwilliger), Beschrijving = "Vrijwilliger", MeervoudBeschrijving = "Vrijwilligers" };
            await rolService.AddRol(rolVrijwilliger);

            var rollen = new Dictionary<RolTypeEnum, Rol>
            {
                { RolTypeEnum.Contactpersoon, rolContactpersoon },
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
