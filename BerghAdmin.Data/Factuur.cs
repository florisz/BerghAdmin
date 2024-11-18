using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class Factuur
{
    public Factuur(int nummer, DateTime datum, int sponsorId)
    {
        Nummer = nummer;
        Datum = datum;
        SponsorId = sponsorId;
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
    public decimal? Bedrag { get; set; }
    public DateTime Datum { get; set; }
    public FactuurStatusEnum FactuurStatus { get; set; }
    public FactuurTypeEnum FactuurType { get; set; } = FactuurTypeEnum.Unknown;
    public Document? EmailTekst { get; set; }
    public Document? FactuurTekst { get; set; }
    public int SponsorId { get; set; }

    [NotMapped]
    public string GetFactuurStatusAsString =>
        FactuurStatusService
            .GetFactuurStatusValues()
            .FirstOrDefault(sv => sv.FactuurStatusValue == FactuurStatus)!
            .FactuurStatusText;
}

public enum FactuurTypeEnum
{
    Unknown = 0,
    Mail = 1,
    Post = 3,
    Pdf = 4,
    Kentaa = 5
}