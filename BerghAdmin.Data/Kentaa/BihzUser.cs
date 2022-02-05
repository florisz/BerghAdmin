namespace BerghAdmin.Data.Kentaa;

public class BihzUser:IBihzResource
{
    public BihzUser()
    { }

    public BihzUser(BihzUser user)
    {
        this.UpdateFrom(user);
    }

    public int Id { get; set; }  // Unique identifier .
    public int UserId { get; set; }  // Unique kentaa identifier for this user.
    public int SiteId { get; set; }     // Unique identifier for the site associated with the user.
    public DateTime CreatieDatum { get; set; }     // The time (ISO 8601 format) when the user was created.
    public DateTime WijzigDatum { get; set; }     // The time (ISO 8601 format) when the user was last updated.
    public string? Voornaam { get; set; }   // The first name of the user.
    public string? Tussenvoegsels { get; set; }    // The infix of the user.OPTIONAL
    public string? Achternaam { get; set; }    // The last name of the user.
    public string? Email { get; set; }    // The email address of the user.
    public string? Adres { get; set; }  // Address line 1 of the user.OPTIONAL
    public string? Adres2 { get; set; }	 // 2	string	Address line 2 of the user.OPTIONAL
    public string? Straat { get; set; }	 // The street name of the user (only when country is NL).OPTIONAL
    public string? HuisNummer { get; set; }	 // The house number of the user (only when country is NL).OPTIONAL
    public string? HuisNummerToevoeging { get; set; }	 // The house number addition of the user (only when country is NL).OPTIONAL
    public string? Postcode { get; set; }	 // The zip code of the user.OPTIONAL
    public string? Woonplaats { get; set; }	 // The city of the user.OPTIONAL
    public string? Land { get; set; }	 // The country of the user (ISO 3166-1 alpha-2 code).OPTIONAL
    public string? Telefoon { get; set; }	 // The phone number of the user.OPTIONAL
    public DateTime? GeboorteDatum { get; set; }	 // The birthday (ISO 8601 format) of the user.OPTIONAL
    public string? Geslacht { get; set; }	 // The gender of the user (male, female, neutral).OPTIONAL
    public int? PersoonId { get; set; }     // id to reference the corresponding Persoon in the BerghAdmin context

    public BihzUser UpdateFrom(BihzUser u)
    {
        UserId = u.UserId;
        SiteId = u.SiteId;
        CreatieDatum = u.CreatieDatum;
        WijzigDatum = u.WijzigDatum;
        Voornaam = u.Voornaam;
        Tussenvoegsels = u.Tussenvoegsels;
        Achternaam = u.Achternaam;
        Email = u.Email;
        Adres = u.Adres;
        Adres2 = u.Adres2;
        Straat = u.Straat;
        HuisNummer = u.HuisNummer;
        HuisNummerToevoeging = u.HuisNummerToevoeging;
        Postcode = u.Postcode;
        Woonplaats = u.Woonplaats;
        Land = u.Land;
        Telefoon = u.Telefoon;
        GeboorteDatum = u.GeboorteDatum;
        Geslacht = u.Geslacht;
        PersoonId = u.PersoonId;

        return this;
    }
}