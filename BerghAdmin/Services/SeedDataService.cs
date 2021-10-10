using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BerghAdmin.Data;
using BerghAdmin.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services
{
    public class SeedDataService : ISeedDataService
    {
        private const string DocumentBasePath = "C:/git/bihz/BerghAdmin/BerghAdmin.Tests/MergeTemplateTests/TestDocumenten"; 
        private readonly ApplicationDbContext _dbContext;
        private readonly IRolService _rolService;

        public SeedDataService(ApplicationDbContext context, IRolService rolService)
        {
            _dbContext = context;
            _rolService = rolService;
        }

        public void Initialize()
        {
            SeedInitialData(_dbContext);

            // set the current user (fzwarteveen)
            // ugly hack needs to be solved later
            var user = _dbContext.Users.Find("8b2197df-e90d-4058-8d14-dc2e389c734e");
        }

        private void SeedInitialData(ApplicationDbContext context)
        {
            var rollen = _rolService.GetRollen();
            if (rollen.Count > 0)
            {
                // no need to seed with testdata, it is there already
                return;
            }

            // Insert all testdata
            var rolAmbassadeur = new Rol { Id = (int) RolTypeEnum.Ambassadeur, Beschrijving = "Ambassadeur", MeervoudBeschrijving = "Ambassadeurs" };
            var rolBegeleider = new Rol { Id = (int) RolTypeEnum.Begeleider, Beschrijving = "Begeleider", MeervoudBeschrijving = "Begeleiders" };
            var rolCommissieLid = new Rol { Id = (int) RolTypeEnum.CommissieLid, Beschrijving = "Commissielid", MeervoudBeschrijving = "Commissieleden" };
            var rolGolfer = new Rol { Id = (int) RolTypeEnum.Golfer, Beschrijving = "Golfer", MeervoudBeschrijving = "Golfers" };
            var rolMailingAbonnee = new Rol { Id = (int) RolTypeEnum.MailingAbonnee, Beschrijving = "Mailing abonnee", MeervoudBeschrijving = "Mailing Abonnees" };
            var rolFietser = new Rol { Id = (int) RolTypeEnum.Fietser, Beschrijving = "Fietser", MeervoudBeschrijving = "Fieters" };
            var rolReserve = new Rol { Id = (int) RolTypeEnum.Reserve, Beschrijving = "Reserve", MeervoudBeschrijving = "Reserves" };
            var rolVriendVan = new Rol { Id = (int) RolTypeEnum.VriendVan, Beschrijving = "Vriend van", MeervoudBeschrijving = "Vrienden van" };
            var rolVrijwilliger = new Rol { Id = (int) RolTypeEnum.Vrijwilliger, Beschrijving = "Vrijwilliger", MeervoudBeschrijving = "Vrijwilligers" };

            context.AddRange(
                new Persoon { 
                    Voorletters = "A. B.",
                    Voornaam = "Appie",
                    Achternaam = "Apenoot", 
                    Adres = "Straat 1", 
                    EmailAdres = "aapnoot@mail.com", 
                    GeboorteDatum = new DateTime(1970, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Amsterdam",
                    Postcode = "1234 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolFietser, rolGolfer}
                },
                new Persoon { 
                    Voorletters = "B.",
                    Voornaam = "Bert",
                    Achternaam = "Bengel", 
                    Adres = "Straat 2", 
                    EmailAdres = "bbengel@mail.com", 
                    GeboorteDatum = new DateTime(1970, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Rotterdam",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolFietser, rolGolfer}
                },
                new Persoon { 
                    Voorletters = "C.",
                    Voornaam = "Charles",
                    Achternaam = "Claassen", 
                    Adres = "Straat 3", 
                    EmailAdres = "bbengel@mail.com", 
                    GeboorteDatum = new DateTime(1945, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "'Heerenberg'",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolFietser, rolAmbassadeur}
                },
                new Persoon { 
                    Voorletters = "BD.",
                    Voornaam = "Dorien",
                    Achternaam = "Dolsma", 
                    Adres = "Straat 4", 
                    EmailAdres = "ddolsma@mail.com", 
                    GeboorteDatum = new DateTime(1970, 1, 1), 
                    Geslacht = GeslachtEnum.Vrouw, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Nieuw Dijk",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolCommissieLid, rolVriendVan, rolMailingAbonnee}
                },
                new Persoon { 
                    Voorletters = "E.",
                    Voornaam = "Edwin",
                    Achternaam = "Evers", 
                    Adres = "Straat 5", 
                    EmailAdres = "eevers@mail.com", 
                    GeboorteDatum = new DateTime(1978, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Beek",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolCommissieLid, rolVriendVan, rolMailingAbonnee}
                },
                new Persoon { 
                    Voorletters = "F.",
                    Voornaam = "Frans",
                    Achternaam = "Franssen", 
                    Adres = "Straat 6", 
                    EmailAdres = "ffranssen@mail.com", 
                    GeboorteDatum = new DateTime(1967, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Kilder",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolVriendVan, rolMailingAbonnee, rolGolfer}
                },
                new Persoon { 
                    Voorletters = "G.",
                    Voornaam = "Gerrit",
                    Achternaam = "Gerritsen", 
                    Adres = "Straat 7", 
                    EmailAdres = "ggerritsen@mail.com", 
                    GeboorteDatum = new DateTime(1987, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Lengel",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolFietser, rolAmbassadeur}
                },
                new Persoon { 
                    Voorletters = "H.",
                    Voornaam = "Harm",
                    Achternaam = "Harmsen", 
                    Adres = "Straat 8", 
                    EmailAdres = "hharmsen@mail.com", 
                    GeboorteDatum = new DateTime(2001, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Oud Dijk",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolGolfer, rolAmbassadeur}
                },
                new Persoon { 
                    Voorletters = "I.",
                    Voornaam = "Ietje",
                    Achternaam = "Ietsma", 
                    Adres = "Straat 9", 
                    EmailAdres = "iietsma@mail.com", 
                    GeboorteDatum = new DateTime(1962, 1, 1), 
                    Geslacht = GeslachtEnum.Vrouw, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Azewijn",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolGolfer, rolAmbassadeur}
                },
                new Persoon { 
                    Voorletters = "J.",
                    Voornaam = "Jantien",
                    Achternaam = "Jorissen", 
                    Adres = "Straat 10", 
                    EmailAdres = "jjorissen@mail.com", 
                    GeboorteDatum = new DateTime(2000, 1, 1), 
                    Geslacht = GeslachtEnum.Vrouw, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Braamt",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolBegeleider, rolCommissieLid, rolVriendVan, rolGolfer, rolAmbassadeur}
                },
                new Persoon { 
                    Voorletters = "K.",
                    Voornaam = "Klaziena",
                    Achternaam = "Kaal", 
                    Adres = "Straat 11", 
                    EmailAdres = "kkaal@mail.com", 
                    GeboorteDatum = new DateTime(2001, 1, 1), 
                    Geslacht = GeslachtEnum.Vrouw, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Loerbeek",
                    Postcode = "4321 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolFietser, rolReserve, rolVrijwilliger}
                },
                new Persoon { 
                    Voorletters = "L.",
                    Voornaam = "Leo",
                    Achternaam = "Leonidas", 
                    Adres = "Straat 12", 
                    EmailAdres = "lleonidas@mail.com", 
                    GeboorteDatum = new DateTime(2002, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Vethuizen",
                    Postcode = "4333 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolFietser, rolVrijwilliger}
                }
            );

            context.AddRange(
                new Document {
                    Name = "Sponsor Factuur",
                    ContentType = ContentTypeEnum.Word,
                    IsMergeTemplate = true,
                    TemplateType = TemplateTypeEnum.Ambassadeur,
                    Content = File.ReadAllBytes($"{DocumentBasePath}/TemplateFactuurSponsor.docx")
                }
            );
            context.SaveChangesWithoutIdentityInsert<Rol>();
        }

    }
}
