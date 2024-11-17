using BerghAdmin.Services.Sponsoren;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace BerghAdmin.Services.Import
{
    public class AmbassadeurImporterService : IImporterService
    {
        // Debiteurnummer	Sponsor	Schrijver 	Fotograaf	2012	2013	2014	2015	2016	2017	2018	2019	2020	2021	2022	2023	2024	OpmerkingLogo	BedragToegezegd	BedragOntvangen	Pakket	DatumAangebracht	Aanbrenger	Adres	Postcode	Plaats	Land	AmbassadeurTelefoon	Mobiel	Contactpersoon	Partner	Emailadres	AanmeldingAmbassadeur	BeeindigingAmbassadeurschap	Compagnon	CompagnonEmail

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

        public async Task ImportDataAsync(Stream csvStream)
        {
            _logger.LogInformation("Start: import data ambassadeurs");
            try
            {
                var configuration = new CsvConfiguration(CultureInfo.InvariantCulture);
                configuration.Delimiter = ";";
                configuration.HasHeaderRecord = true;

                var memoryStream = await ReadExternalStreamAsync(csvStream);
                _logger.LogInformation("Incoming stream converted to memory stream");
                using (var textReader = new StreamReader(memoryStream, Encoding.UTF8))
                using (var csv = new CsvReader(textReader, configuration))
                {
                    var records = csv.GetRecordsAsync<CsvAmbassadeurRecord>();
                    await foreach (var record in records)
                    {
                        _logger.LogInformation($"record of deb: {record.Debiteurnummer} read from csv stream");
                        Persoon contactPersoon1 = null;
                        Persoon contactPersoon2 = null;
                        Persoon compagnon = null;
                        // first find out if contactpersoon and compagnon already exists and if not create it
                        if (!String.IsNullOrEmpty(record.Emailadres))
                        {
                            contactPersoon1 = await SaveContactpersoon(record);
                            _logger.LogInformation($"Contact persoon 1 with email: {contactPersoon1.EmailAdres} saved");
                        }
                        if (!String.IsNullOrEmpty(record.CompagnonEmail))
                        {
                            compagnon = await SaveCompagnon(record);
                            _logger.LogInformation($"Compagnon with email: {compagnon.EmailAdres} saved");
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
                            FactuurVerzendWijze = FactuurVerzendwijzeEnum.Mail,

                            // Donateur specific fields
                            EmailAdres = record.Emailadres,
                            Adres = record.Adres,
                            Postcode = record.Postcode,
                            Plaats = record.Plaats,
                            Land = record.Land,
                            Telefoon = record.AmbassadeurTelefoon,
                            Mobiel = record.Mobiel,
                            Opmerkingen = "",
                            IsVerwijderd = false
                        };
                        // vul alle MagazineJaren
                        AddMagazineJaren(ambassadeur, record);

                        _logger.LogInformation($"Save Ambassadeur: {ambassadeur.Naam}");

                        await _ambassadeurService.SaveAsync(ambassadeur);
                        _logger.LogInformation($"Ambassadeur met debiteurnummer: {ambassadeur.DebiteurNummer} ingelezen");
                    }
                }
                _logger.LogInformation("Alle ambassadeurs ingelezen");
                var ambassadeurs = await _ambassadeurService.GetAll();
                var personen = await _persoonService.GetPersonen();
                var contactPersoonRol = _rolService.GetRolById(RolTypeEnum.Contactpersoon);
                var compagnonPersoonRol = _rolService.GetRolById(RolTypeEnum.Compagnon);
                _logger.LogInformation($"# personen={personen.Count}");
                _logger.LogInformation($"# ambassadeurs={ambassadeurs.ToList().Count}");
                _logger.LogInformation($"# contact personen={contactPersoonRol.Personen.Count}");
                _logger.LogInformation($"# compagnons={compagnonPersoonRol.Personen.Count}");
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Exception {ex.Message} thrown");
                throw new ApplicationException($"Csv import exception {ex.Message}", ex);
            }
        }

        private async Task<MemoryStream> ReadExternalStreamAsync(Stream csvStream)
        {
            // convert incoming stream into memory stream
            var memoryStream = new MemoryStream();
            await csvStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0; // Reset the position of the memory stream to the beginning
            csvStream.Close();

            return memoryStream;
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
            if (contactPersoon == null)
            {
                contactPersoon = new Persoon
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
            }
            contactPersoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Contactpersoon));
            await _persoonService.SavePersoonAsync(contactPersoon);

            return contactPersoon;
        }

        private async Task<Persoon> SaveCompagnon(CsvAmbassadeurRecord record)
        {
            var compagnon = await _persoonService.GetByEmailAdres(record.CompagnonEmail);
            if (compagnon == null)
            {
                compagnon = new Persoon
                {
                    Geslacht = GeslachtEnum.Onbekend,
                    Achternaam = record.Compagnon,
                    EmailAdres = record.CompagnonEmail,
                    IsVerwijderd = false,
                    Rollen = new HashSet<Rol>()
                };
            }
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
