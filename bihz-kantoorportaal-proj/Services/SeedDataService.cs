using System;
using System.Collections.Generic;
using bihz.kantoorportaal.Data;
using bihz.kantoorportaal.DbContexts;

namespace bihz.kantoorportaal.Services
{
    public class SeedDataService : ISeedDataService
    {
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
            var rolAmbassadeur = new Rol { Beschrijving = "Ambassadeur"};
            var rolBegeleider = new Rol { Beschrijving = "Begeleider"};
            var rolCommissieLid = new Rol { Beschrijving = "Commissielid"};
            var rolGolfer = new Rol { Beschrijving = "Golfer"};
            var rolMailingAbonnee = new Rol { Beschrijving = "Mailing abonnee"};
            var rolRenner = new Rol { Beschrijving = "Renner"};
            var rolReserve = new Rol { Beschrijving = "Reserve"};
            var rolVriendVan = new Rol { Beschrijving = "Vriend van"};

            context.AddRange(
                new Persoon { 
                    Voorletters = "A. B.",
                    Voornaam = "Appie",
                    Achternaam = "Apenoot", 
                    Adres = "Straat 1", 
                    EmailAdres = "aapenoot@mail.com", 
                    GeboorteDatum = new DateTime(1970, 1, 1), 
                    Geslacht = GeslachtEnum.Man, 
                    Land = "Nederland", 
                    Mobiel = "06-12345678", 
                    Plaats = "Amsterdam",
                    Postcode = "1234 AB",
                    Telefoon = "onbekend",
                    Rollen = new HashSet<Rol>() {rolRenner, rolGolfer}
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
                    Rollen = new HashSet<Rol>() {rolRenner, rolGolfer}
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
                    Rollen = new HashSet<Rol>() {rolRenner, rolAmbassadeur}
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
                    Rollen = new HashSet<Rol>() {rolRenner, rolAmbassadeur}
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
                    Rollen = new HashSet<Rol>() {rolRenner, rolReserve}
                }
            );

            context.SaveChanges();
        }
    }
}
