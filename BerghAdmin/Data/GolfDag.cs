namespace BerghAdmin.Data;

public class GolfDag : Evenement
{
    public DateTime GeplandeDatum { get; set; }
    public string? Locatie { get; set; }
    public string? Omschrijving { get; set; }
}
