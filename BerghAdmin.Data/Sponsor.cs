using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class Sponsor : Donateur
{
    public Sponsor()
    {
        ContactPersoon = new Persoon();
    }
    public int? DebiteurNummer { get; set; }
    public string Naam { get; set; }
    public Persoon ContactPersoon { get; set; }
    public Persoon? Compagnon { get; set; }

    [NotMapped]
    public string ContactPersoonVolledigeNaam => ContactPersoon.VolledigeNaam;
    [NotMapped]
    public string ContactPersoonTelefoon => $"m:{ContactPersoon.Telefoon}/t:{ContactPersoon.Mobiel}";
    [NotMapped]
    public string ContactPersoonEmailAdres => ContactPersoon.EmailAdres;
    [NotMapped]
    public string? CompagnonVolledigeNaam => Compagnon?.VolledigeNaam;
    [NotMapped]
    public string? CompagnonTelefoon => $"m:{Compagnon?.Telefoon}/t:{Compagnon?.Mobiel}";
    [NotMapped]
    public string? CompagnonEmailAdres => Compagnon?.EmailAdres;
}
