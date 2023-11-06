namespace BerghAdmin.Data;

public class Ambassadeur : Sponsor
{
    public decimal? ToegezegdBedrag { get; set; }
    public decimal? TotaalBedrag { get; set; }
    public DateTime? DatumAangebracht { get; set; }
    public PakketEnum Pakket { get; set; }
    public DateTime Fax { get; set; }
}
