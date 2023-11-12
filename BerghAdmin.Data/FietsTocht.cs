using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class Fietstocht : Evenement
{
    public ICollection<Persoon> Deelnemers { get; set; } = new List<Persoon>();
    [NotMapped]
    public int AantalDeelnemers { get { return Deelnemers.Count; } }
}

public class FietstochtListItem
{
    public int Id { get; set; }
    public string Titel { get; set; }
}