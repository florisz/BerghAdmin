using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BerghAdmin.Data;
using CsvHelper;

namespace BerghAdmin.Services.Import
{
    public class DataImporterService : IDataImporterService
    {
        private readonly IPersoonService _persoonService;
        private readonly IRolService _rolService;
        public DataImporterService(IPersoonService persoonService, IRolService rolService)
        {
            _persoonService = persoonService;
            _rolService = rolService;
        }

        public void ImportData(Stream csvData)
        {
            try
            {
                // Create an instance of StreamReader to read from a file.
                // The using statement also closes the StreamReader.
                using (StreamReader reader = new StreamReader(csvData))
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<OldDataRecord>().ToList<OldDataRecord>();
                    foreach(var record in records)
                    {
                        var persoon = new Persoon();
                        persoon.Geslacht = string.IsNullOrEmpty(record.GeslachtId)? GeslachtEnum.Onbekend : record.GeslachtId == "1"? GeslachtEnum.Man : GeslachtEnum.Vrouw;
                        persoon.Voornaam = record.Voornaam;
                        persoon.Voorletters = record.Voorletters;
                        persoon.Tussenvoegsel = record.Tussenvoegsel;
                        persoon.Achternaam = record.Achternaam;
                        persoon.Adres = record.Adres;
                        persoon.Postcode = record.Postcode;
                        persoon.Plaats = record.Plaats;
                        persoon.Land = record.Land;
                        persoon.GeboorteDatum = ConvertToDateType(record.GeboorteDag, record.GeboorteMaand, record.GeboorteJaar);

                        persoon.Telefoon = record.Telefoon;
                        persoon.Mobiel = record.Mobiel;
                        persoon.EmailAdres = record.Emailadres;
                        persoon.IsVerwijderd = record.IsVerwijderd == "1"? true : false;
                        persoon.Rollen = new HashSet<Rol>();
                        if (record.IsRenner == "1")
                        {
                            persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Fietser));
                        }
                        if (record.IsBegeleider == "1")
                        {
                            persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.Begeleider));
                        }
                        if (record.IsReserve == "1")
                        {
                            // TO BE DONE!
                        }
                        if (record.IsCommissielid == "1")
                        {
                            persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.CommissieLid));
                        }
                        if (record.IsVriendvan == "1")
                        {
                            persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.VriendVan));
                        }
                        if (record.IsMailingAbonnee == "1")
                        {
                            persoon.Rollen.Add(_rolService.GetRolById(RolTypeEnum.MailingAbonnee));
                        }

                        _persoonService.SavePersoon(persoon);
                    }
                }
            }
            catch (Exception)
            {
                // Let the user know what went wrong.
            }
        }
        private DateTime? ConvertToDateType(string day, string month, string year)
        {
            try
            {
                var date = DateTime.ParseExact($"{day}/{month}/{year}", "d/M/yyyy", null);
                return date;
            }
            catch(Exception e)
            {
                return null;
            }

        }
    }
}
