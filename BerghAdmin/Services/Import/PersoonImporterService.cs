using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO.Abstractions;
using System.Text;

namespace BerghAdmin.Services.Import
{
    public class PersoonImporterService : IImporterService
    {
        // Debiteurnummer	Sponsor	Schrijver 	Fotograaf	2012	2013	2014	2015	2016	2017	2018	2019	2020	2021	2022	2023	2024	OpmerkingLogo	BedragToegezegd	BedragOntvangen	Pakket	DatumAangebracht	Aanbrenger	Adres	Postcode	Plaats	Land	AmbassadeurTelefoon	Mobiel	Contactpersoon	Partner	Emailadres	AanmeldingAmbassadeur	BeeindigingAmbassadeurschap	Compagnon	CompagnonEmail

        private readonly IPersoonService _persoonService;
        private readonly IRolService _rolService;
        public PersoonImporterService(IPersoonService persoonService, IRolService rolService)
        {
            _persoonService = persoonService;
            _rolService = rolService;
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
                    var records = csv.GetRecordsAsync<CsvPersoonRecord>();
                    await foreach (var record in records)
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

                        await _persoonService.SavePersoonAsync(persoon);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Csv import exception {ex.Message}", ex);
            }

            return;
        }
        private static DateTime? ConvertToDateType(string day, string month, string year)
        {
            if (DateTime.TryParseExact($"{day}/{month}/{year}", "d/M/yyyy", null, DateTimeStyles.None, out var date))
                return date;
         
            return null;
        }

    }
}
