namespace BerghAdmin.Data;

public class Organisatie : Donateur
{
    public Organisatie() : base()
    { }

    public string? Naam { get; set; }
    public Persoon? ContactPersoon { get; set; } 
}
