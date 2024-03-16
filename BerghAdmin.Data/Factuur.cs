using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class Factuur
{
    public Factuur(int nummer, DateTime datum)
    {
        Nummer = nummer;
        Datum = datum;
    }

    public int Id { get; set; }
    public int Nummer { get; set; }
    [NotMapped]
    public string FactuurNummer
    {
        get
        {
            return Nummer.ToString("00000");
        }
    }
    public string? Omschrijving { get; set; }
    public float? Bedrag { get; set; }
    public DateTime Datum { get; set; }
    public bool IsVerzonden { get; set; }
    public FactuurTypeEnum FactuurType { get; set; } = FactuurTypeEnum.Unknown;
    public Document? EmailTekst { get; set; }
    public Document? FactuurTekst { get; set; }

    // TO DO: work out logic
    public bool IsBetaald()
    {
        return true; 
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