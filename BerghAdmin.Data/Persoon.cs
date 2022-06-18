using BerghAdmin.Data.Kentaa;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public enum GeslachtEnum
{
    Onbekend,
    Man,
    Vrouw
}

public class Persoon : Donateur
{
    public Persoon() : base()
    {
        Geslacht = GeslachtEnum.Onbekend;
        Rollen = new HashSet<Rol>();
        EmailAdres = "";
    }

    public GeslachtEnum Geslacht { get; set; }
    public string? Voorletters { get; set; }
    public string? Voornaam { get; set; }
    public string? Achternaam { get; set; }
    public string? Tussenvoegsel { get; set; }
    public DateTime? GeboorteDatum { get; set; }
    public HashSet<Rol> Rollen { get; set; } = new();
    public ICollection<VerzondenMail> Geadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<VerzondenMail> ccGeadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<VerzondenMail> bccGeadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<Evenement>? IsDeelnemerVan { get; set; }
    public BihzActie? BihzActie { get; set; }
    public BihzUser? BihzUser { get; set; }
    public BihzProject? Project { get; set; }
    public bool IsReserve { get; set; } = false;

    [NotMapped]
    public string GetRollenAsString
        => string.Join(", ", Rollen.Select(r => r.Beschrijving));

    [NotMapped]
    public string VolledigeNaam
        => string.Join(" ", new string?[] { 
                                Voornaam, 
                                string.IsNullOrEmpty(Voorletters)? "" : $"({Voorletters})", 
                                Tussenvoegsel, 
                                Achternaam } 
        );
    
}

