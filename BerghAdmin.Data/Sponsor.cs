using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class Sponsor : Donateur
{
    public string DebiteurNummer { get; set; }
    public string Naam { get; set; }
    public Persoon? ContactPersoon1 { get; set; }
    public Persoon? ContactPersoon2 { get; set; }
    public Persoon? Compagnon { get; set; }

    [NotMapped]
    public string? ContactPersoon1VolledigeNaam => ContactPersoon1?.VolledigeNaam;
    [NotMapped]
    public string? ContactPersoon1Telefoon => GetTelefoon(ContactPersoon1);
    [NotMapped]
    public string? ContactPersoon1EmailAdres => ContactPersoon1?.EmailAdres;
    [NotMapped]
    public string? ContactPersoon2VolledigeNaam => ContactPersoon2?.VolledigeNaam;
    [NotMapped]
    public string ContactPersoon2Telefoon => GetTelefoon(ContactPersoon2);
    [NotMapped]
    public string? ContactPersoon2EmailAdres => ContactPersoon2?.EmailAdres;
    [NotMapped]
    public string? CompagnonVolledigeNaam => Compagnon?.VolledigeNaam;
    [NotMapped]
    public string? CompagnonTelefoon => GetTelefoon(Compagnon);
    [NotMapped]
    public string? CompagnonEmailAdres => Compagnon?.EmailAdres;

    private string GetTelefoon(Persoon? persoon)
    {
        if (persoon == null) return "";
        
        var telefoon = string.IsNullOrEmpty(persoon.Telefoon)? "<?>": persoon.Telefoon;
        var mobiel = string.IsNullOrEmpty(persoon.Mobiel)? "<?>": persoon.Mobiel;

        return $"t:{telefoon}/m:{mobiel}";
    }
}
