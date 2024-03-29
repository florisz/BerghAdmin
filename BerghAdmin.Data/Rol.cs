#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace BerghAdmin.Data;

public class Rol
{
    public int Id { get; set; }
    public string Beschrijving { get; set; }
    public string MeervoudBeschrijving { get; set; }
    public HashSet<Persoon> Personen { get; set; }
}

public class RolListItem
{
    public int Id { get; set; }
    public string Beschrijving { get; set; }
}