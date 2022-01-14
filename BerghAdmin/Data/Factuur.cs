namespace BerghAdmin.Data;

public class Factuur
{
    public Factuur()
    {
        Nummer = GenerateFactuurNummer();
    }

    public int Id { get; set; }
    public string Nummer { get; set; }
    public string? Omschrijving { get; set; }
    public float? Bedrag { get; set; }
    public DateTime? Datum { get; set; }
    public bool IsVerzonden { get; set; }
    public FactuurTypeEnum FactuurType { get; set; }
    public Document? EmailTekst { get; set; }
    public Document? FactuurTekst { get; set; }
        
    // TO DO: work out logic
    public bool IsBetaald()
    {
        return true; 
    }

    private string GenerateFactuurNummer()
    {
        // TO BE DONE: temporary solution
        return "2022-1";
    }
}

public enum FactuurTypeEnum
{
    Unknown = 0,
    Mail = 1,
    Post = 3,
    Pdf = 4,
    Kentaa = 5
}