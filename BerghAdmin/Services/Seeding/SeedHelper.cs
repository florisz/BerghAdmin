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
            var rolCompagnon = new Rol { Id = Convert.ToInt32(RolTypeEnum.Compagnon), Beschrijving = "Compagnon", MeervoudBeschrijving = "Compagnons" };
            await rolService.AddRol(rolCompagnon);

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

        public static async Task InsertJaarMagazines(IMagazineService magazineService)
        {
            await magazineService.DeleteAll();

            await magazineService.AddMagazine(new MagazineJaar { Id = 1, Jaar = "2012" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 2, Jaar = "2013" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 3, Jaar = "2014" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 4, Jaar = "2015" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 5, Jaar = "2016" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 6, Jaar = "2017" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 7, Jaar = "2018" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 8, Jaar = "2019" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 9, Jaar = "2020" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 10, Jaar = "2021" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 11, Jaar = "2022" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 12, Jaar = "2023" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 13, Jaar = "2024" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 14, Jaar = "2025" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 15, Jaar = "2026" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 16, Jaar = "2027" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 17, Jaar = "2028" });
            await magazineService.AddMagazine(new MagazineJaar { Id = 18, Jaar = "2029" });
        }  
    }
}
