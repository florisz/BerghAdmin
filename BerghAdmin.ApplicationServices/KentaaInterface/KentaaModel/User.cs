using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
#pragma warning disable IDE1006 // Naming Styles

public record UserResponse(User User);

public record User
(
    int id, // Unique identifier for this user.
    int site_id, // Unique identifier for the site associated with the user.
    DateTime created_at, // The time (ISO 8601 format) when the user was created.
    DateTime updated_at, // The time (ISO 8601 format) when the user was last updated.
    string first_name, // The first name of the user.
    string infix, // The infix of the user.OPTIONAL
    string last_name, // The last name of the user.
    string email, // The email address of the user.
    string avatar_url, // The URL to the avatar image of the user.OPTIONAL
    string address, // Address line 1 of the user.OPTIONAL
    string address2, // 2	string	Address line 2 of the user.OPTIONAL
    string street, // The street name of the user (only when country is NL).OPTIONAL
    string house_number, // The house number of the user (only when country is NL).OPTIONAL
    string house_number_addition, // The house number addition of the user (only when country is NL).OPTIONAL
    string zipcode, // The zip code of the user.OPTIONAL
    string city, // The city of the user.OPTIONAL
    string country, // The country of the user (ISO 3166-1 alpha-2 code).OPTIONAL
    string phone, // The phone number of the user.OPTIONAL
    DateTime birthday, // The birthday (ISO 8601 format) of the user.OPTIONAL
    string gender, // The gender of the user (male, female, neutral).OPTIONAL
    string locale // The locale when the user was created (nl, en, de, fr, etc).
) : IResource
{
    public IBihzResource Map()
    {
        return new BihzUser
        {
            UserId = this.id,
            SiteId = this.site_id,
            CreatieDatum = this.created_at,
            WijzigDatum = this.updated_at,
            Voornaam = this.first_name,
            Tussenvoegsels = this.infix,
            Achternaam = this.last_name,
            Email = this.email,
            Adres = this.address,
            Adres2 = this.address2,
            Straat = this.street,
            HuisNummer = this.house_number,
            HuisNummerToevoeging = this.house_number_addition,
            Postcode = this.zipcode,
            Woonplaats = this.city,
            Land = this.country,
            Telefoon = this.phone,
            GeboorteDatum = this.birthday,
            Geslacht = this.gender,
        };
    }
}
