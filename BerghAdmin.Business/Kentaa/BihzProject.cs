using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Data.Kentaa;

public class BihzProject
{
    public BihzProject()
    { }

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

    public BihzProject(KM.Project project)
    {
        Map(project);
    }

    public void Map(KM.Project project)
    {
        ProjectId = project.Id;
        SiteId = project.SiteId;
        Slug = project.Slug;
        CreatieDatum = project.CreatedAt;
        WijzigDatum = project.UpdatedAt;
        ExterneReferentie = project.ExternalReference;
        Titel = project.Title;
        Omschrijving = project.Description;
        DoelBedrag = project.TargetAmount;
        TotaalBedrag = project.TotalAmount;
        AantalDonaties = project.TotalDonations;
        DoelBedragBereikt = project.TargetAmountAchieved;
        Zichtbaar = project.Visible;
        Gesloten = project.Closed;
        Beeindigd = project.Ended;
        EindDatum = project.EndDate;
        Url = project.Url;
        DonatieUrl = project.DonateUrl;
    }
}