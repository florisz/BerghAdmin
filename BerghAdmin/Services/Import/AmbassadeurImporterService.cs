using BerghAdmin.Data;
using BerghAdmin.Services.Documenten;
using BerghAdmin.Services.Evenementen;
using BerghAdmin.Services.Sponsoren;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.SqlServer.Server;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO.Abstractions;
using System.Text;

namespace BerghAdmin.Services.Import
{
    public class AmbassadeurImporterService : IImporterService
    {
        string[] DateOnlyFormats = { "dd-MM-yyyy", "d-MM-yyyy", "dd-M-yyyy", "d-M-yyyy" };
        string[] DateTimeFormats = { "dd-MM-yyyy HH:mm", "d-MM-yyyy HH:mm", "dd-M-yyyy HH:mm", "d-M-yyyy HH:mm" };

        private readonly IRolService _rolService;
        private readonly IPersoonService _persoonService;
        private readonly IAmbassadeurService _ambassadeurService;
        private readonly IMagazineService _magazineService;
        private readonly ILogger _logger;

        public AmbassadeurImporterService(IRolService rolService, IPersoonService persoonService, IAmbassadeurService ambassadeurService, IMagazineService magazineService, ILogger<AmbassadeurImporterService> logger)
        {
            _rolService = rolService;
            _persoonService = persoonService;
            _ambassadeurService = ambassadeurService;
            _magazineService = magazineService;
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


                using (var textReader = new StreamReader(csvData, Encoding.UTF8))
                using (var csv = new CsvReader(textReader, configuration))
                {
                    var records = csv.GetRecordsAsync<CsvAmbassadeurRecord>();
                    await foreach (var record in records)
                    {
                        Persoon contactPersoon1 = null;
                        Persoon contactPersoon2 = null;
                        Persoon compagnon = null;
                        // first find out if contactpersoon and compagnon already exists and if not create it
                        if (!String.IsNullOrEmpty(record.Emailadres))
                        {
                            contactPersoon1 = await SaveContactpersoon(record);
                        }
                        if (!String.IsNullOrEmpty(record.CompagnonEmail))
                        {
                            compagnon = await SaveCompagnon(record);
                        }

                        // now create the ambassadeur
                        var ambassadeur = new Ambassadeur
                        {
                            // Ambassadeur specific fields
                            AangebrachtDoor = record.Aanbrenger,
                            DatumAangebracht = ParseDatum(record.DatumAangebracht, DateTimeFormats),
                            DatumAanmelding = ParseDatum(record.AanmeldingAmbassadeur, DateOnlyFormats),
                            DatumBeeindiging = ParseDatum(record.BeeindigingAmbassadeurschap, DateOnlyFormats),
                            MagazijnSchrijver = record.Schrijver,
                            MagazijnFotograaf = record.Fotograaf,
                            OpmerkingenLogo = record.OpmerkingLogo,
                            Partner = record.Partner,
                            Pakket = PakketEnum.Ambassadeur,
                            ToegezegdBedrag = Decimal.Parse(record.BedragToegezegd),
                            TotaalBedrag = Decimal.Parse(record.BedragOntvangen),

                            // Sponsor specific fields
                            DebiteurNummer = record.Debiteurnummer,
                            Naam = record.Sponsor,
                            ContactPersoon1 = contactPersoon1,
                            ContactPersoon2 = contactPersoon2,
                            Compagnon = compagnon,

                            // Donateur specific fields
                            Telefoon = record.AmbassadeurTelefoon,
                            IsVerwijderd = false
                        };
                        // vul alle MagazineJaren
                        AddMagazineJaren(ambassadeur, record);

                        _logger.LogTrace($"Save Ambassadeur: {ambassadeur.Naam}");

                        await _ambassadeurService.SaveAsync(ambassadeur);
                        _logger.LogTrace($"Ambassadeur met debiteurnummer: {ambassadeur.DebiteurNummer} ingelezen");
                    }
                }
                _logger.LogTrace("Alle ambassadeurs ingelezen");
                var ambassadeurs = await _ambassadeurService.GetAll();
                var personen = await _persoonService.GetPersonen();
                var contactPersoonRol = _rolService.GetRolById(RolTypeEnum.Contactpersoon);
                var compagnonPersoonRol = _rolService.GetRolById(RolTypeEnum.Compagnon);
                _logger.LogTrace($"# personen={personen.Count}");
                _logger.LogTrace($"# ambassadeurs={ambassadeurs.ToList().Count}");
                _logger.LogTrace($"# contact personen={contactPersoonRol.Personen.Count}");
                _logger.LogTrace($"# compagnons={compagnonPersoonRol.Personen.Count}");
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Csv import exception {ex.Message}", ex);
            }
        }

        private DateTime? ParseDatum(string datumString, string[] formats)
        {
            if (String.IsNullOrEmpty(datumString))
            {
                return null;
            }
            return DateTime.ParseExact(datumString, formats, CultureInfo.InvariantCulture);
        }

        private Decimal? ParseDecimal(string decimalString)
        {
            var provider = new CultureInfo("nl-NL");
            var style = NumberStyles.AllowLeadingWhite | 
                        NumberStyles.Currency | 
                        NumberStyles.AllowDecimalPoint | 
                        NumberStyles.AllowThousands;

            if (String.IsNullOrEmpty(decimalString))
            {
                return null;
            }
            return Decimal.Parse(decimalString, style, provider);
        }

        private void AddMagazineJaren(Ambassadeur ambassadeur, CsvAmbassadeurRecord record)
        {
            var recordType = record.GetType();
            var jaarProperties = 
                recordType
                    .GetProperties()
                    .Where(p => p.Name.StartsWith("MJ20"));
            foreach (var jaarProperty in jaarProperties)
            {
                var value = jaarProperty.GetValue(record);
                if (value != null)
                {
                    string stringValue = value as string;
                    var magazine = _magazineService.GetMagazineByJaar(stringValue);
                    if (magazine == null)
                    {
                        // TO DO: log warning
                        continue;
                    }
                    ambassadeur.AddMagazineJaar(magazine);
                }
            }
        }

        private async Task<Persoon> SaveContactpersoon(CsvAmbassadeurRecord record)
        {
            var contactPersoon = await _persoonService.GetByEmailAdres(record.Emailadres);
            contactPersoon ??= new Persoon
                {
                    Geslacht = GeslachtEnum.Onbekend,
                    Voornaam = record.Voornaam,
                    Tussenvoegsel = record.Tussenvoegsel,
                    Achternaam = record.Achternaam,
                    Adres = record.Adres,
                    Postcode = record.Postcode,
                    Plaats = record.Plaats,
                    Land = record.Land,
                    Mobiel = record.Mobiel,
                    EmailAdres = record.Emailadres,
                    IsVerwijderd = false,
                    Rollen = new HashSet<Rol>()
                };
            contactPersoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Contactpersoon));
            await _persoonService.SavePersoonAsync(contactPersoon);

            return contactPersoon;
        }

        private async Task<Persoon> SaveCompagnon(CsvAmbassadeurRecord record)
        {
            var compagnon = await _persoonService.GetByEmailAdres(record.CompagnonEmail);
            compagnon ??= new Persoon
                {
                    Geslacht = GeslachtEnum.Onbekend,
                    Achternaam = record.Compagnon,
                    EmailAdres = record.CompagnonEmail,
                    IsVerwijderd = false,
                    Rollen = new HashSet<Rol>()
                };
            compagnon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Compagnon));
            await _persoonService.SavePersoonAsync(compagnon);

            return compagnon;
        }

        private static DateTime? ConvertToDateType(string day, string month, string year)
        {
            if (DateTime.TryParseExact($"{day}/{month}/{year}", "d/M/yyyy", null, DateTimeStyles.None, out var date))
                return date;
         
            return null;
        }
    }
}
