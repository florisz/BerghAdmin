using CsvHelper.Configuration.Attributes;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace BerghAdmin.Services.Import
{
    public class CsvPersoonRecord
    {
        [Name("Id")]
        public string Id { get; set; }
        [Name("IsRenner")]
        public string IsRenner { get; set; }
        [Name("IsBegeleider")]
        public string IsBegeleider { get; set; }
        [Name("IsReserve")]
        public string IsReserve { get; set; }
        [Name("IsCommissielid")]
        public string IsCommissielid { get; set; }
        [Name("IsVriendvan")]
        public string IsVriendvan { get; set; }
        [Name("IsMailingAbonnee")]
        public string IsMailingAbonnee { get; set; }
        [Name("GeslachtId")]
        public string GeslachtId { get; set; }
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
        [Name("Emailadres")]
        public string Emailadres { get; set; }
        [Name("Nummer")]
        public string Nummer { get; set; }
        [Name("GeboorteDag")]
        public string GeboorteDag { get; set; }
        [Name("GeboorteMaand")]
        public string GeboorteMaand { get; set; }
        [Name("GeboorteJaar")]
        public string GeboorteJaar { get; set; }
        [Name("ToonIntroductie")]
        public string ToonIntroductie { get; set; }
        [Name("Kledingmaten")]
        public string Kledingmaten { get; set; }
        [Name("IsVerwijderd")]
        public string IsVerwijderd { get; set; }
    }
}


