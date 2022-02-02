namespace BerghAdmin.Data.Kentaa;

public class BihzProject
{
    public BihzProject()
    { }

    public BihzProject(BihzProject newProject)
    {
        UpdateFrom(newProject);
    }

    public int Id { get; set; }                     // Unique internal id
    public string? Slug { get; set; }               // Unique identifier for this project, used to create a nice project URL.
    public int ProjectId { get; set; }              // Unique identifier for the site associated with the project.
    public int SiteId { get; set; }                 // Unique identifier for the site associated with the project.
    public DateTime CreatieDatum { get; set; }      // The time (ISO 8601 format) when the team was created.
    public DateTime WijzigDatum { get; set; }       // The time (ISO 8601 format) when the team was last updated.
    public string? ExterneReferentie { get; set; }  // External reference for the project.OPTIONAL
    public string? Titel { get; set; }              // Title for the project.
    public string? Omschrijving { get; set; }       // Description for the project.
    public decimal DoelBedrag { get; set; }         // The target amount for the project.
    public decimal TotaalBedrag { get; set; }       // The amount that was donated to the project (with decimals).
    public int AantalDonaties { get; set; }         // The number of donations for the project.
    public bool DoelBedragBereikt { get; set; }     // Indicates whether the project target amount is achieved or not.
    public bool Zichtbaar { get; set; }             // Indicates whether the project is visible or not.
    public bool Gesloten { get; set; }              // Indicates whether the project is closed or not.
    public bool Beeindigd { get; set; }             // Indicates whether the project is ended or not.
    public DateTime EindDatum { get; set; }         // The countdown date (ISO 8601 format) for this project.OPTIONAL
    public string? Url { get; set; }                // The URL to the project.
    public string? DonatieUrl { get; set; }	        // The URL to directly make a donation to the project.
    public int? EvenementId { get; set; }           // id to reference the corresponding Evenment in the BerghAdmin context

    public static BihzProject UpdateFrom(BihzProject p)
    {
        return new BihzProject
        {
            Slug = p.Slug,
            ProjectId = p.ProjectId,
            SiteId = p.SiteId,
            CreatieDatum = p.CreatieDatum,
            WijzigDatum = p.WijzigDatum,
            ExterneReferentie = p.ExterneReferentie,
            Titel = p.Titel,
            Omschrijving = p.Omschrijving,
            DoelBedrag = p.DoelBedrag,
            TotaalBedrag = p.TotaalBedrag,
            AantalDonaties = p.AantalDonaties,
            DoelBedragBereikt = p.DoelBedragBereikt,
            Zichtbaar = p.Zichtbaar,
            Gesloten = p.Gesloten,
            Beeindigd = p.Beeindigd,
            EindDatum = p.EindDatum,
            Url = p.Url,
            DonatieUrl = p.DonatieUrl,
            EvenementId = p.EvenementId,
        };
    }
}