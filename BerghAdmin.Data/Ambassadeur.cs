namespace BerghAdmin.Data;

public class Ambassadeur : Sponsor
{
    public string? AangebrachtDoor { get; set; }
    public DateTime? DatumAangebracht { get; set; }
    public DateTime? DatumAanmelding { get; set; }
    public DateTime? DatumBeeindiging { get; set; }
    public ICollection<MagazineJaar> MagazineJaren { get; set; } = new List<MagazineJaar>();
    public ICollection<Factuur> Facturen { get; set; } = new List<Factuur>();
    public string? Fax { get; set; }
    public string? MagazijnSchrijver { get; set; }
    public string? MagazijnFotograaf { get; set; }
    public string? OpmerkingenLogo { get; set; }
    public string? Partner { get; set; }
    public PakketEnum Pakket { get; set; }
    public decimal? ToegezegdBedrag { get; set; }
    public decimal? TotaalBedrag { get; set; }
    public void AddMagazineJaar(MagazineJaar magazineJaar)
    {   
        if (MagazineJaren == null)
        {
            MagazineJaren = new List<MagazineJaar>();
        }
        if (MagazineJaren.FirstOrDefault(mj => mj.Jaar == magazineJaar.Jaar) == null)
        {
            MagazineJaren.Add(magazineJaar);
        }
    }

    public void DeleteMagazineJaar(MagazineJaar magazineJaar)
    {
        if (MagazineJaren == null)
        {
            MagazineJaren = new List<MagazineJaar>();
        }
        if (MagazineJaren.Contains(magazineJaar))
        {
            MagazineJaren.Remove(magazineJaar);
        }
    }
}
