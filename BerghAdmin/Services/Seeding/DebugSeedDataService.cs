using BerghAdmin.Services.Documenten;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Sponsoren;
using Microsoft.Extensions.Options;

namespace BerghAdmin.Services.Seeding;

public class DebugSeedDataService : ISeedDataService
{
    private readonly SeedSettings _settings;
    private readonly IRolService _rolService;
    private readonly IFietstochtenService _fietstochtenService;
    private readonly IGolfdagenService _golfdagenService;
    private readonly IPersoonService _persoonService;
    private readonly ISponsorService _sponsorService;
    private readonly IAmbassadeurService _ambassadeurService;
    private readonly IDocumentService _documentService;

    public DebugSeedDataService(
        IRolService rolService,
        IFietstochtenService fietstochtenService,
        IGolfdagenService golfdagenService,
        IPersoonService persoonService,
        ISponsorService sponsorService,
        IAmbassadeurService ambassadeurService,
        IDocumentService documentService,
        IOptions<SeedSettings> settings)
    {
        _settings = settings.Value;
        _rolService = rolService;
        _fietstochtenService = fietstochtenService;
        _golfdagenService = golfdagenService;
        _persoonService = persoonService;
        // TO DO: omzetten naar Golfdagsponsor service
        _sponsorService = sponsorService;
        _ambassadeurService = ambassadeurService;
        _documentService = documentService;
    }

    public async Task SeedInitialData()
    {
        await InsertDocumenten();
        if (SeedHelper.DatabaseHasData(_rolService))
        {
            return;
        }

        var rollen = await SeedHelper.InsertRollen(_rolService);

        await InsertTestPersonen(rollen);
        await InsertFietstochten();
        await InsertSponsoren();
        await InsertGolfdagen();
        await InsertDocumenten();
    }

    private async Task InsertTestPersonen(Dictionary<RolTypeEnum, Rol> rollen)
    {
        var persoon = new Persoon
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
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.MailingAbonnee] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
        {
            Voorletters = "B.D.",
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
            Telefoon = "0314-123456",
            IsVerwijderd = true,
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.CommissieLid], rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.Contactpersoon] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
            Telefoon = "0314-123456",
            IsReserve = true,
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.CommissieLid], rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.Contactpersoon] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
            Telefoon = "0314-123456",
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.Contactpersoon], rollen[RolTypeEnum.Golfer] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser], rollen[RolTypeEnum.Contactpersoon] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.Contactpersoon] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.Contactpersoon] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Begeleider], rollen[RolTypeEnum.CommissieLid], rollen[RolTypeEnum.VriendVan], rollen[RolTypeEnum.Golfer], rollen[RolTypeEnum.Contactpersoon] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
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
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
        {
            Voorletters = "R.",
            Voornaam = "Rudy",
            Achternaam = "Reserve",
            Adres = "Straat 13",
            EmailAdres = "rreserve@mail.com",
            GeboorteDatum = new DateTime(1989, 1, 1),
            Geslacht = GeslachtEnum.Man,
            Land = "Nederland",
            Mobiel = "06-12345678",
            Plaats = "Didam",
            Postcode = "6901 AB",
            Telefoon = "onbekend",
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Fietser] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
        {
            Voorletters = "V.",
            Voornaam = "Veronica",
            Achternaam = "Verwijderd",
            Adres = "Straat 14",
            EmailAdres = "vverwijderd@mail.com",
            GeboorteDatum = new DateTime(1984, 1, 1),
            Geslacht = GeslachtEnum.Vrouw,
            Land = "Nederland",
            Mobiel = "06-12345678",
            Plaats = "Loil",
            Postcode = "6904 SD",
            Telefoon = "onbekend",
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Vrijwilliger] }
        };
        await _persoonService.SavePersoonAsync(persoon);

        persoon = new Persoon
        {
            Voorletters = "F.",
            Voornaam = "Floris",
            Achternaam = "Zwarteveen",
            Adres = "Straat 42",
            EmailAdres = "fzwarteveen@gmail.com",
            GeboorteDatum = new DateTime(1984, 1, 1),
            Geslacht = GeslachtEnum.Man,
            Land = "Nederland",
            Mobiel = "06-12345678",
            Plaats = "Beek",
            Postcode = "7037 CA",
            Telefoon = "onbekend",
            Rollen = new HashSet<Rol>() { rollen[RolTypeEnum.Vrijwilliger], rollen[RolTypeEnum.Fietser] }
        };
        await _persoonService.SavePersoonAsync(persoon);
    }

    private async Task InsertFietstochten()
    {
        try
        {
            // add alle fietstochten
            var fietstocht = new Fietstocht()
            {
                Id = 0,
                GeplandeDatum = new DateTime(2015, 5, 9),
                Titel = "Klaver Vier Tocht 2015"
            };
            await _fietstochtenService.SaveAsync(fietstocht);
            fietstocht = new Fietstocht()
            {
                Id = 0,
                GeplandeDatum = new DateTime(2019, 5, 24),
                Titel = "Bergh-Leipzig-Bergh 2019"
            };
            await _fietstochtenService.SaveAsync(fietstocht);
            fietstocht = new Fietstocht()
            {
                Id = 0,
                GeplandeDatum = new DateTime(2023, 5, 3),
                Titel = "Hanzetocht 2023",
                KentaaProjectId = 17805
            };
            await _fietstochtenService.SaveAsync(fietstocht);

            // AddAsync deelnemers to fietstochten
            var persoon = await _persoonService.GetByEmailAdres("appie@aapnootmies.com");
            if (persoon != null)
                fietstocht.Deelnemers.Add(persoon);
            await _fietstochtenService.SaveAsync(fietstocht);

            persoon = await _persoonService.GetByEmailAdres("bert@aapnootmies.com");
            if (persoon != null)
                fietstocht.Deelnemers.Add(persoon);
            await _fietstochtenService.SaveAsync(fietstocht);

            persoon = await _persoonService.GetByEmailAdres("chappie@aapnootmies.com");
            if (persoon != null)
                fietstocht.Deelnemers.Add(persoon);
            await _fietstochtenService.SaveAsync(fietstocht);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task InsertSponsoren()
    {
        // assumption: InsertTestPersonen() has been called
        var persoon1 = await _persoonService.GetByEmailAdres("ddolsma@mail.com");
        var persoon2 = await _persoonService.GetByEmailAdres("eevers@mail.com");
        var persoon3 = await _persoonService.GetByEmailAdres("ffranssen@mail.com");

        // insert an Ambassadeur
        var ambassadeur = new Ambassadeur()
        {
            Id = 0,
            DatumAangebracht = new DateTime(2015, 5, 9),
            Naam = "De Gulle Gever",
            Pakket = PakketEnum.Ambassadeur,
            ToegezegdBedrag = 2000,
            TotaalBedrag = 16000, 
            ContactPersoon = persoon1!,
            Compagnon = persoon2
        };
        await _ambassadeurService.SaveAsync(ambassadeur);

        // insert an Ambassadeur
        ambassadeur = new Ambassadeur()
        {
            Id = 0,
            DatumAangebracht = new DateTime(2020, 2, 23),
            Naam = "De Uitdeler",
            Pakket = PakketEnum.Ambassadeur,
            ToegezegdBedrag = 2000,
            TotaalBedrag = 6000,
            ContactPersoon = persoon2!
        };
        await _ambassadeurService.SaveAsync(ambassadeur);

        // insert an Ambassadeur
        ambassadeur = new Ambassadeur()
        {
            Id = 0,
            DatumAangebracht = new DateTime(2003, 10, 30),
            Naam = "Meer is Beter",
            Pakket = PakketEnum.Ambassadeur,
            ToegezegdBedrag = 2000,
            TotaalBedrag = 40000,
            ContactPersoon = persoon3!
        };
        await _ambassadeurService.SaveAsync(ambassadeur);

        // insert a GolfdagSponsor
        var sponsor = new GolfdagSponsor()
        {
            Id = 0,
            Naam = "Mepgraag",
            ContactPersoon = persoon1!,
            Opmerkingen = "Voorzichtig",
            DebiteurNummer = 1234,
            Mobiel = "06-12345678"
        };
        await _sponsorService.SaveAsync<GolfdagSponsor>(sponsor);
        // insert a GolfdagSponsor
        sponsor = new GolfdagSponsor()
        {
            Id = 0,
            Naam = "In de put",
            ContactPersoon = persoon2!,
            Opmerkingen = "Niet vrolijk",
            DebiteurNummer = 5678,
            Mobiel = "06-12345678"
        };
        await _sponsorService.SaveAsync<GolfdagSponsor>(sponsor);
        // insert a GolfdagSponsor
        sponsor = new GolfdagSponsor()
        {
            Id = 0,
            Naam = "Holeinone",
            ContactPersoon = persoon3!,
            Opmerkingen = "Raak!",
            DebiteurNummer = 9012,
            Mobiel = "06-12345678"
        };
        await _sponsorService.SaveAsync<GolfdagSponsor>(sponsor);
    }

    private async Task InsertGolfdagen()
    {
        // assumption: InsertSponsoren() has been called
        try
        {
            // add nieuwe golfdag
            var golfdag = new Golfdag()
            {
                Id = 0,
                GeplandeDatum = new DateTime(2023, 10, 11),
                Titel = "Golfdag November 2023",
                Locatie = "Golfbaan Borghees",

            };
            await _golfdagenService.SaveAsync(golfdag);

            // add golfdag sponsoren
            var sponsor = await _sponsorService.GetByNaam<GolfdagSponsor>("Sponsor 1");
            if (sponsor != null)
                golfdag.Sponsoren.Add(sponsor);
            await _golfdagenService.SaveAsync(golfdag);

            // add golfdag deelnemers
            var persoon = await _persoonService.GetByEmailAdres("chappie@aapnootmies.com");
            if (persoon != null)
                golfdag.Deelnemers.Add(persoon);
            await _golfdagenService.SaveAsync(golfdag);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private async Task InsertDocumenten()
    {
        var document = new Document
        {
            Name = "Sponsor Factuur",
            IsMergeTemplate = true,
            ContentType = ContentTypeEnum.Template,
            DocumentType = DocumentTypeEnum.Word,
            TemplateType = TemplateTypeEnum.Ambassadeur,
            Content = File.ReadAllBytes($"{_settings.DocumentBasePath}/TemplateFactuurSponsor.docx"),
            Created = DateTime.Now,
            Owner = "Henk"
        };
        await _documentService.SaveDocument(document);
        document = new Document
        {
            Name = "Ambassadeur Factuur Template",
            IsMergeTemplate = true,
            ContentType = ContentTypeEnum.Template,
            DocumentType = DocumentTypeEnum.Word,
            TemplateType = TemplateTypeEnum.Ambassadeur,
            Content = File.ReadAllBytes($"{_settings.DocumentBasePath}/AmbassadeurFactuurTemplate.docx"),
            Created = DateTime.Now,
            Owner = "Wilbert"
        };
        await _documentService.SaveDocument(document);
    }


}
