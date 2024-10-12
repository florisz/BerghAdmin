using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace BerghAdmin.Services.Import
{
    public class PersoonImporterService : IImporterService
    {
        private readonly IPersoonService _persoonService;
        private readonly IRolService _rolService;
        private readonly ILogger _logger;

        public PersoonImporterService(IPersoonService persoonService, IRolService rolService, ILogger<PersoonImporterService> logger)
        {
            _persoonService = persoonService;
            _rolService = rolService;
            _logger = logger;
        }

        public async Task ImportDataAsync(Stream csvData)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
                configuration.Delimiter = ";";
                configuration.HasHeaderRecord = true;

                _logger.LogInformation("Aanvang personen inlezen");
                await LogOverview();

                using (var textReader = new StreamReader(csvData, Encoding.UTF8))
                using (var csv = new CsvReader(textReader, configuration))
                {
                    var records = csv.GetRecordsAsync<CsvPersoonRecord>();
                    await foreach (var record in records)
                    {
                        var persoon = await _persoonService.GetByEmailAdres(record.Emailadres);
                        if (persoon == null)
                        {
                            _logger.LogInformation($"Persoon met emailadres {record.Emailadres} bestaat nog niet. Nieuwe persoon wordt aangemaakt.");
                            persoon = CreateNewPersoon(record);
                        }
                        else
                        {
                            _logger.LogInformation($"Persoon {persoon.VolledigeNaam} met emailadres {persoon.EmailAdres} bestaat al. Persoon wordt bijgewerkt.");
                            persoon = UpdatePersoon(persoon, record);
                        }

                        await _persoonService.SavePersoonAsync(persoon);
                    }
                    _logger.LogInformation("Alle personen ingelezen");
                    await LogOverview();
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Csv import exception {ex.Message}", ex);
            }

            return;
        }

        private Persoon CreateNewPersoon(CsvPersoonRecord record)
        {
            var persoon = new Persoon
            {
                Geslacht = string.IsNullOrEmpty(record.GeslachtId) ? GeslachtEnum.Onbekend : record.GeslachtId == "1" ? GeslachtEnum.Man : GeslachtEnum.Vrouw,
                Voornaam = record.Voornaam,
                Voorletters = record.Voorletters,
                Tussenvoegsel = record.Tussenvoegsel,
                Achternaam = record.Achternaam,
                Adres = record.Adres,
                Postcode = record.Postcode,
                Plaats = record.Plaats,
                Land = record.Land,
                GeboorteDatum = ConvertToDateType(record.GeboorteDag, record.GeboorteMaand, record.GeboorteJaar),

                Telefoon = record.Telefoon,
                Mobiel = record.Mobiel,
                EmailAdres = record.Emailadres,
                KledingMaten = record.Kledingmaten,
                IsVerwijderd = record.IsVerwijderd == "1",
                Nummer = record.Nummer,
                Rollen = new HashSet<Rol>()
            };
            if (record.IsRenner == "1")
            {
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Fietser) ?? throw new ArgumentNullException("Id", "GetRole Fietser"));
            }
            if (record.IsBegeleider == "1")
            {
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Begeleider) ?? throw new ArgumentNullException("Id", "GetRole Begeleider"));
            }
            if (record.IsReserve == "1")
            {
                persoon.IsReserve = true;
            }
            if (record.IsCommissielid == "1")
            {
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.CommissieLid) ?? throw new ArgumentNullException("Id", "GetRole CommissieLid"));
            }
            if (record.IsVriendvan == "1")
            {
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.VriendVan) ?? throw new ArgumentNullException("Id", "GetRole VriendVan"));
            }
            if (record.IsMailingAbonnee == "1")
            {
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.MailingAbonnee) ?? throw new ArgumentNullException("Id", "GetRole MailingAbonne"));
            }

            return persoon;
        }

        private Persoon UpdatePersoon(Persoon persoon, CsvPersoonRecord record)
        {
            _logger.LogInformation($"Persoon {persoon.VolledigeNaamMetRollenEnEmail} wordt bijgewerkt...");

            var geslacht = string.IsNullOrEmpty(record.GeslachtId) ? GeslachtEnum.Onbekend : record.GeslachtId == "1" ? GeslachtEnum.Man : GeslachtEnum.Vrouw;
            if (persoon.Geslacht != geslacht) {
                _logger.LogInformation($"Geslacht is bijgewerkt van '{persoon.Geslacht}' naar '{geslacht}'");
                persoon.Geslacht = geslacht;
            }
            if (persoon.Voornaam != record.Voornaam)
            {
                _logger.LogInformation($"Voornaam is bijgewerkt van '{persoon.Voornaam}' naar '{record.Voornaam}'");
                persoon.Voornaam = record.Voornaam;
            }
            if (persoon.Voorletters != record.Voorletters)
            {
                _logger.LogInformation($"Voorletters is bijgewerkt van '{persoon.Voorletters}' naar '{record.Voorletters}'");
                persoon.Voorletters = record.Voorletters;
            }
            if (persoon.Tussenvoegsel != record.Tussenvoegsel)
            {
                _logger.LogInformation($"Tussenvoegsel is bijgewerkt van '{persoon.Tussenvoegsel}' naar '{record.Tussenvoegsel}'");
                persoon.Tussenvoegsel = record.Tussenvoegsel;
            }
            if (persoon.Achternaam != record.Achternaam)
            {
                _logger.LogInformation($"Achternaam is bijgewerkt van '{persoon.Achternaam}' naar '{record.Achternaam}'");
                persoon.Achternaam = record.Achternaam;
            }
            if (persoon.Adres != record.Adres)
            {
                _logger.LogInformation($"Adres is bijgewerkt van '{persoon.Adres}' naar '{record.Adres}'");
                persoon.Adres = record.Adres;
            }
            if (persoon.Postcode != record.Postcode)
            {
                _logger.LogInformation($"Postcode is bijgewerkt van '{persoon.Postcode}' naar '{record.Postcode}'");
                persoon.Postcode = record.Postcode;
            }
            if (persoon.Plaats != record.Plaats)
            {
                _logger.LogInformation($"Plaats is bijgewerkt van '{persoon.Plaats}' naar '{record.Plaats}'");
                persoon.Plaats = record.Plaats;
            }
            if (persoon.Land != record.Land)
            {
                _logger.LogInformation($"Land is bijgewerkt van '{persoon.Land}' naar '{record.Land}'");
                persoon.Land = record.Land;
            }
            var geboorteDatum = ConvertToDateType(record.GeboorteDag, record.GeboorteMaand, record.GeboorteJaar);
            if (persoon.GeboorteDatum != geboorteDatum)
            {
                _logger.LogInformation($"Geboortedatum is bijgewerkt van '{persoon.GeboorteDatum}' naar '{geboorteDatum}'");
                persoon.GeboorteDatum = geboorteDatum;
            }
            if (persoon.Telefoon != record.Telefoon)
            {
                   _logger.LogInformation($"Telefoon is bijgewerkt van '{persoon.Telefoon}' naar '{record.Telefoon}'");
                persoon.Telefoon = record.Telefoon;
            }
            if (persoon.Mobiel != record.Mobiel)
            {
                _logger.LogInformation($"Mobiel is bijgewerkt van '{persoon.Mobiel}' naar '{record.Mobiel}'");
                persoon.Mobiel = record.Mobiel;
            }
            if (persoon.KledingMaten != record.Kledingmaten)
            {
                _logger.LogInformation($"Kledingmaten is bijgewerkt van '{persoon.KledingMaten}' naar '{record.Kledingmaten}'");
                persoon.KledingMaten = record.Kledingmaten;
            }
            if (persoon.IsVerwijderd != (record.IsVerwijderd == "1"))
            {
                _logger.LogInformation($"IsVerwijderd is bijgewerkt van '{persoon.IsVerwijderd}' naar '{record.IsVerwijderd}'");
                persoon.IsVerwijderd = record.IsVerwijderd == "1";
            }
            if (persoon.Nummer != record.Nummer)
            {
                _logger.LogInformation($"Nummer is bijgewerkt van '{persoon.Nummer}' naar '{record.Nummer}'");
                persoon.Nummer = record.Nummer;
            }
            if (record.IsRenner == "1" && persoon.Rollen.Contains(_rolService.GetRolById(RolTypeEnum.Fietser)) == false)
            {
                _logger.LogInformation($"Rol Fietser is toegevoegd");
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Fietser) ?? throw new ArgumentNullException("Id", "GetRole Fietser"));
            }
            if (record.IsBegeleider == "1" && persoon.Rollen.Contains(_rolService.GetRolById(RolTypeEnum.Begeleider)) == false)
            {
                _logger.LogInformation($"Rol Begeleider is toegevoegd");
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Begeleider) ?? throw new ArgumentNullException("Id", "GetRole Begeleider"));
            }
            if (record.IsReserve == "1" && persoon.IsReserve == false)
            {
                _logger.LogInformation($"IsReserve is toegevoegd");
                persoon.IsReserve = true;
            }
            if (record.IsCommissielid == "1" && persoon.Rollen.Contains(_rolService.GetRolById(RolTypeEnum.CommissieLid)) == false)
            {
                _logger.LogInformation($"Rol CommissieLid is toegevoegd");
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.CommissieLid) ?? throw new ArgumentNullException("Id", "GetRole CommissieLid"));
            }
            if (record.IsVriendvan == "1" && persoon.Rollen.Contains(_rolService.GetRolById(RolTypeEnum.VriendVan)) == false)
            {
                _logger.LogInformation($"Rol VriendVan is toegevoegd");
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.VriendVan) ?? throw new ArgumentNullException("Id", "GetRole VriendVan"));
            }
            if (record.IsMailingAbonnee == "1" && persoon.Rollen.Contains(_rolService.GetRolById(RolTypeEnum.MailingAbonnee)) == false)
                {
                _logger.LogInformation($"Rol MailingAbonnee is toegevoegd");
                persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.MailingAbonnee) ?? throw new ArgumentNullException("Id", "GetRole MailingAbonnee"));
            }

            return persoon;
        }
        private async Task LogOverview()
        {
            var rolTypes = new RolTypeEnum[] { 
                RolTypeEnum.Compagnon,
                RolTypeEnum.Contactpersoon, 
                RolTypeEnum.Begeleider,
                RolTypeEnum.CommissieLid,
                RolTypeEnum.Fietser,
                RolTypeEnum.Golfer,
                RolTypeEnum.MailingAbonnee,
                RolTypeEnum.VriendVan,
                RolTypeEnum.Vrijwilliger
            };

            var personen = await _persoonService.GetPersonen();
            _logger.LogInformation($"Totaal aantal personen: {personen.Count}");
            foreach (var rolType in rolTypes)
            {
                var rol = _rolService.GetRolById(rolType);
                _logger.LogInformation($"Totaal in rol '{rol.Beschrijving}' :{rol.Personen?.Count}");
            }
        }

        private static DateTime? ConvertToDateType(string day, string month, string year)
        {
            if (DateTime.TryParseExact($"{day}/{month}/{year}", "d/M/yyyy", null, DateTimeStyles.None, out var date))
                return date;
         
            return null;
        }

    }
}
