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
}
