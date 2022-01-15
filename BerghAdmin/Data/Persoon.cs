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
    public Persoon()
    {
        Geslacht = GeslachtEnum.Onbekend;
        Rollen = new HashSet<Rol>();
    }
    public GeslachtEnum Geslacht { get; set; }
    public string? Voorletters { get; set; }
    public string? Voornaam { get; set; }
    public string? Achternaam { get; set; }
    public string? Tussenvoegsel { get; set; }
    public DateTime? GeboorteDatum { get; set; }
    public string? Telefoon  { get; set; }
    public string? Mobiel  { get; set; }
    public string? EmailAdres  { get; set; } 
    public string? EmailAdresExtra  { get; set; } 
    public HashSet<Rol> Rollen { get; set; } = new();
    public ICollection<VerzondenMail> Geadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<VerzondenMail> ccGeadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<VerzondenMail> bccGeadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<Evenement>? IsDeelnemerVan { get; set; }
        
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

