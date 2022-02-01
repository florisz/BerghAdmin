using BerghAdmin.Data.Kentaa;

using System.Text.Json.Serialization;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class UserResponse
{
    [JsonPropertyName("User")]
    public User Data { get; set; }
}

public class User : Resource
{
    [JsonPropertyName("id")]
    public int Id { get; set; }  // Unique identifier for this user.

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }     // Unique identifier for the site associated with the user.

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }     // The time (ISO 8601 format) when the user was created.

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }     // The time (ISO 8601 format) when the user was last updated.

    [JsonPropertyName("first_name")]
    public string FirstName { get; set; }   // The first name of the user.

    [JsonPropertyName("infix")]
    public string Infix { get; set; }    // The infix of the user.OPTIONAL

    [JsonPropertyName("last_name")]
    public string LastName { get; set; }    // The last name of the user.

    [JsonPropertyName("email")]
    public string Email { get; set; }    // The email address of the user.

    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; }   // The URL to the avatar image of the user.OPTIONAL

    [JsonPropertyName("address")]
    public string Address { get; set; }  // Address line 1 of the user.OPTIONAL

    [JsonPropertyName("address2")]
    public string Address2 { get; set; }	 // 2	string	Address line 2 of the user.OPTIONAL

    [JsonPropertyName("street")]
    public string Street { get; set; }	 // The street name of the user (only when country is NL).OPTIONAL

    [JsonPropertyName("house_number")]
    public string HouseNumber { get; set; }	 // The house number of the user (only when country is NL).OPTIONAL

    [JsonPropertyName("house_number_addition")]
    public string HouseNumberAddition { get; set; }	 // The house number addition of the user (only when country is NL).OPTIONAL

    [JsonPropertyName("zipcode")]
    public string Zipcode { get; set; }	 // The zip code of the user.OPTIONAL

    [JsonPropertyName("city")]
    public string City { get; set; }	 // The city of the user.OPTIONAL

    [JsonPropertyName("country")]
    public string Country { get; set; }	 // The country of the user (ISO 3166-1 alpha-2 code).OPTIONAL

    [JsonPropertyName("phone")]
    public string Phone { get; set; }	 // The phone number of the user.OPTIONAL

    [JsonPropertyName("birthday")]
    public DateTime Birthday { get; set; }	 // The birthday (ISO 8601 format) of the user.OPTIONAL

    [JsonPropertyName("gender")]
    public string Gender { get; set; }	 // The gender of the user (male, female, neutral).OPTIONAL

    [JsonPropertyName("locale")]
    public string Locale { get; set; }   // The locale when the user was created (nl, en, de, fr, etc).

    public BihzUser Map()
    {
        return new BihzUser
        {
            UserId = this.Id,
            SiteId = this.SiteId,
            CreatieDatum = this.CreatedAt,
            WijzigDatum = this.UpdatedAt,
            Voornaam = this.FirstName,
            Tussenvoegsels = this.Infix,
            Achternaam = this.LastName,
            Email = this.Email,
            Adres = this.Address,
            Adres2 = this.Address2,
            Straat = this.Street,
            HuisNummer = this.HouseNumber,
            HuisNummerToevoeging = this.HouseNumberAddition,
            Postcode = this.Zipcode,
            Woonplaats = this.City,
            Land = this.Country,
            Telefoon = this.Phone,
            GeboorteDatum = this.Birthday,
            Geslacht = this.Gender,
        };
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

