using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class Golfdag : Evenement
{
    public string? Locatie { get; set; }
    public ICollection<Persoon> Deelnemers { get; set; } = new List<Persoon>();
    public ICollection<GolfdagSponsor> Sponsoren{ get; set; } = new List<GolfdagSponsor>();
    [NotMapped]
    public int AantalDeelnemers { get { return Deelnemers.Count; } }
}
