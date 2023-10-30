using BerghAdmin.Data.Kentaa;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public abstract class Evenement
{
    public int Id { get; set; }
    public string? Titel { get; set; }
    public DateTime GeplandeDatum { get; set; }
    public HashSet<Persoon> Deelnemers { get; set;} = new();
    public int? KentaaProjectId { get; set; }
    [NotMapped]
    public int AantalDeelnemers { get { return Deelnemers.Count; } }
}

public enum EvenementTypeEnum
{
    Unknown,
    Fietstocht,
    Golfdag
}

