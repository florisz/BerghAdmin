namespace BerghAdmin.Data;

public abstract class Evenement
{
    public int Id { get; set; }
    public string? Titel { get; set; }
    public DateTime GeplandeDatum { get; set; }
    public int? KentaaProjectId { get; set; }
}
