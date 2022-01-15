using System.IO;
using BerghAdmin.DbContexts;
using BerghAdmin.Services.Evenementen;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BerghAdmin.Services;

public class SeedDataService : ISeedDataService
{
    private const string DocumentBasePath = ""; 
    private readonly IServiceProvider _serviceProvider;
    private readonly SeedSettings _settings;

    public SeedDataService(IServiceProvider serviceProvider, IOptions<SeedSettings> settings)
    {
        _serviceProvider = serviceProvider;
        _settings = settings.Value;
    }

    public void SeedInitialData()
    {
        using (var scope = _serviceProvider.CreateScope())
        using (var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>())
        {
            if (dbContext == null)
            {
                throw new InvalidOperationException("dbcontext can not be null");
            }

            if (!DatabaseIsEmpty(scope))
            {
                return;
            }

            var rollen = InsertRollen(dbContext);

            InsertTestPersonen(dbContext, rollen);

            InsertUsers(scope, dbContext, rollen);

            InsertEvenementen(scope, dbContext);

            //InsertDocumenten(dbContext);
        }
    }


    private ApplicationDbContext GetDbContext(IServiceScope scope)
    {

        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        if (context == null)
        {
            throw new ApplicationException("can not create application db context in SeedDataService");
        }

        return context; 
    }

    private bool DatabaseIsEmpty(IServiceScope scope)
    {
        var rolService = (IRolService)scope.ServiceProvider.GetRequiredService<IRolService>();
        var rollen = rolService.GetRollen();
        if (rollen.Count > 0)
        {
            // no need to seed with testdata, it is there already
            return false;
        }
        
        return true;
    }

    private Dictionary<RolTypeEnum, Rol> InsertRollen(ApplicationDbContext dbContext)
    {        

        var rolAmbassadeur = new Rol { Id = RolTypeEnum.Ambassadeur, Beschrijving = "Ambassadeur", MeervoudBeschrijving = "Ambassadeurs" };
        dbContext.Add(rolAmbassadeur);

        var rolBegeleider = new Rol { Id = RolTypeEnum.Begeleider, Beschrijving = "Begeleider", MeervoudBeschrijving = "Begeleiders" };
        dbContext.Add(rolBegeleider);

        var rolCommissieLid = new Rol { Id = RolTypeEnum.CommissieLid, Beschrijving = "Commissielid", MeervoudBeschrijving = "Commissieleden" };
        dbContext.Add(rolCommissieLid);

        var rolGolfer = new Rol { Id = RolTypeEnum.Golfer, Beschrijving = "Golfer", MeervoudBeschrijving = "Golfers" };
        dbContext.Add(rolGolfer);

        var rolMailingAbonnee = new Rol { Id = RolTypeEnum.MailingAbonnee, Beschrijving = "Mailing abonnee", MeervoudBeschrijving = "Mailing Abonnees" };
        dbContext.Add(rolMailingAbonnee);

        var rolFietser = new Rol { Id = RolTypeEnum.Fietser, Beschrijving = "Fietser", MeervoudBeschrijving = "Fieters" };
        dbContext.Add(rolFietser);

        var rolVriendVan = new Rol { Id = RolTypeEnum.VriendVan, Beschrijving = "Vriend van", MeervoudBeschrijving = "Vrienden van" };
        dbContext.Add(rolVriendVan);

        var rolVrijwilliger = new Rol { Id = RolTypeEnum.Vrijwilliger, Beschrijving = "Vrijwilliger", MeervoudBeschrijving = "Vrijwilligers" };
        dbContext.Add(rolVrijwilliger);

        dbContext.SaveChanges();

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

    private void InsertTestPersonen(ApplicationDbContext dbContext, Dictionary<RolTypeEnum, Rol> rollen)
    {
        dbContext.AddRange(
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Golfer] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Golfer] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Ambassadeur] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.CommissieLid], rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.MailingAbonnee] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.CommissieLid], rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.MailingAbonnee] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.MailingAbonnee], rollen[RolTypeEnum.Golfer] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Ambassadeur] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.Ambassadeur] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.Ambassadeur] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Begeleider], rollen[RolTypeEnum.CommissieLid], rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.Ambassadeur] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Vrijwilliger] }
            },
            new Persoon
            {
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
                Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Vrijwilliger] }
            }
        );

        dbContext.SaveChanges();
    }

    private void InsertUsers(IServiceScope scope, ApplicationDbContext dbContext, Dictionary<RolTypeEnum, Rol> rollen)
    {
        var persoon = new Persoon
        {
            Voorletters = "F.",
            Voornaam = "Floris",
            Achternaam = "Zwarteveen",
            Adres = "Berkenlaan 12",
            EmailAdres = "fzwarteveen@mail.com",
            GeboorteDatum = new DateTime(2002, 1, 1),
            Geslacht = GeslachtEnum.Man,
            Land = "Nederland",
            Mobiel = "06-12345678",
            Plaats = "Beek",
            Postcode = "7037 CA",
            Telefoon = "onbekend",
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Vrijwilliger] }
        };
        dbContext.Add(persoon);
        dbContext.SaveChanges();

        var user = new User
        {
            CurrentPersoonId = persoon.Id,
            Name = "fzwarteveen@gmail.com",
            Roles = new string[] { "admin" },
            UserName = "fzwarteveen@gmail.com",
            Email = "fzwarteveen@gmail.com",
            AccessFailedCount = 0,
            EmailConfirmed = true,
            LockoutEnabled = false,
            LockoutEnd = null,
            PhoneNumber = "",
            PhoneNumberConfirmed = true,
            TwoFactorEnabled = false
        };

        var userManager = (UserManager<User>)scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var result = userManager.CreateAsync(user, "qwerty@123").Result;
        if (result.Succeeded)
        {
        }
    }

    private void InsertDocumenten(ApplicationDbContext dbContext)
    {
        dbContext.AddRange(
            new Document
            {
                Name = "Sponsor Factuur",
                ContentType = ContentTypeEnum.Word,
                IsMergeTemplate = true,
                TemplateType = TemplateTypeEnum.Ambassadeur,
                Content = File.ReadAllBytes($"{_settings.DocumentBasePath}/TemplateFactuurSponsor.docx"),
                Owner = "Henk"
            }
        );

        dbContext.SaveChanges();
    }

    private void InsertEvenementen(IServiceScope scope, ApplicationDbContext dbContext)
    {
        var evenementService = (IEvenementService) scope.ServiceProvider.GetRequiredService<IEvenementService>();
        var fietstocht = new FietsTocht()
        {
            Id = 0,
            GeplandJaar = new DateTime(2032, 1, 1),
            Naam = "Hanzetocht"
        };
        evenementService.Save(fietstocht);
    }
}
