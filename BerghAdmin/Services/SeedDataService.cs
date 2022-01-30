using BerghAdmin.Authorization;
using BerghAdmin.DbContexts;
using BerghAdmin.Services.Evenementen;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using System.Security.Claims;

namespace BerghAdmin.Services;

public class SeedDataService : ISeedDataService
{
    private readonly SeedSettings _settings;
    private readonly ApplicationDbContext _dbContext;
    private readonly IRolService _rolService;
    private readonly IEvenementService _evenementService;

    public SeedDataService(
        ApplicationDbContext dbContext,
        IRolService rolService,
        IEvenementService evenementService,
        IOptions<SeedSettings> settings)
    {
        this._settings = settings.Value;
        this._dbContext = dbContext;
        this._rolService = rolService;
        this._evenementService = evenementService;
    }

    public async Task SeedInitialData()
    {
        if (DatabaseHasData())
        {
            return;
        }

        var rollen = await InsertRollen();

        await InsertTestPersonen(rollen);
        await InsertEvenementen();
        //await InsertDocumenten();
    }

    private bool DatabaseHasData()
        => this._rolService
            .GetRollen()
            .Any();

    private async Task<Dictionary<RolTypeEnum, Rol>> InsertRollen()
    {
        var rolAmbassadeur = new Rol { Id = RolTypeEnum.Ambassadeur, Beschrijving = "Ambassadeur", MeervoudBeschrijving = "Ambassadeurs" };
        await _dbContext.AddAsync(rolAmbassadeur);

        var rolBegeleider = new Rol { Id = RolTypeEnum.Begeleider, Beschrijving = "Begeleider", MeervoudBeschrijving = "Begeleiders" };
        await _dbContext.AddAsync(rolBegeleider);

        var rolCommissieLid = new Rol { Id = RolTypeEnum.CommissieLid, Beschrijving = "Commissielid", MeervoudBeschrijving = "Commissieleden" };
        await _dbContext.AddAsync(rolCommissieLid);

        var rolGolfer = new Rol { Id = RolTypeEnum.Golfer, Beschrijving = "Golfer", MeervoudBeschrijving = "Golfers" };
        await _dbContext.AddAsync(rolGolfer);

        var rolMailingAbonnee = new Rol { Id = RolTypeEnum.MailingAbonnee, Beschrijving = "Mailing abonnee", MeervoudBeschrijving = "Mailing Abonnees" };
        await _dbContext.AddAsync(rolMailingAbonnee);

        var rolFietser = new Rol { Id = RolTypeEnum.Fietser, Beschrijving = "Fietser", MeervoudBeschrijving = "Fieters" };
        await _dbContext.AddAsync(rolFietser);

        var rolVriendVan = new Rol { Id = RolTypeEnum.VriendVan, Beschrijving = "Vriend van", MeervoudBeschrijving = "Vrienden van" };
        await _dbContext.AddAsync(rolVriendVan);

        var rolVrijwilliger = new Rol { Id = RolTypeEnum.Vrijwilliger, Beschrijving = "Vrijwilliger", MeervoudBeschrijving = "Vrijwilligers" };
        await _dbContext.AddAsync(rolVrijwilliger);

        await _dbContext.SaveChangesAsync();

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

    private async Task InsertTestPersonen(Dictionary<RolTypeEnum, Rol> rollen)
    {
        await _dbContext.AddRangeAsync(
            new Persoon
            {
                Voorletters = "A. B.",
                Voornaam = "Appie",
                Achternaam = "Apenoot",
                Adres = "Straat 1",
                EmailAdres = "appie@aapnootmies.com",
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
                EmailAdres = "bert@aapnootmies.com",
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
                Voornaam = "Chappie",
                Achternaam = "Claassen",
                Adres = "Straat 3",
                EmailAdres = "chappie@aapnootmies.com",
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

        _dbContext.SaveChanges();
    }

    private async Task InsertDocumenten()
    {
        await _dbContext.AddRangeAsync(
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

        await _dbContext.SaveChangesAsync();
    }

    private async Task InsertEvenementen()
    {
        var fietstocht = new FietsTocht()
        {
            Id = 0,
            GeplandeDatum = new DateTime(2023, 5, 3),
            Titel = "Hanzetocht 2023"
        };
        await this._evenementService.Save(fietstocht);
    }

}
