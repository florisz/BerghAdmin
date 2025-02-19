using CsvHelper.Configuration.Attributes;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace BerghAdmin.Services.Import
{
    public class CsvPersoonRecord
    {
        [Name("Groep")]
        public string Groep { get; set; }
        [Name("Geslacht")]
        public string Geslacht { get; set; }
        [Name("Voorletters")]
        public string Voorletters { get; set; }
        [Name("Voornaam")]
        public string Voornaam { get; set; }
        [Name("Tussenvoegsel")]
        public string Tussenvoegsel { get; set; }
        [Name("Achternaam")]
        public string Achternaam { get; set; }
        [Name("Adres")]
        public string Adres { get; set; }
        [Name("Postcode")]
        public string Postcode { get; set; }
        [Name("Plaats")]
        public string Plaats { get; set; }
        [Name("Land")]
        public string Land { get; set; }
        [Name("Telefoon")]
        public string Telefoon { get; set; }
        [Name("Mobiel")]
        public string Mobiel { get; set; }
        [Name("E-mailadres")]
        public string Emailadres { get; set; }
        [Name("Nummer")]
        public string Nummer { get; set; }
        [Name("Geboortedatum")]
        public string Geboortedatum { get; set; }
        [Name("Kledingmaten")]
        public string Kledingmaten { get; set; }
    }
}


