using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable IDE1006 // Naming Styles

public record ProjectResponse(Project Project);

public record Project
(
    int id,  // Unique identifier for this project.
    string slug,     // Unique identifier for this project, used to create a nice project URL.
    int site_id,     // Unique identifier for the site associated with the project.
    int segment_id,  // Unique identifier for the segment associated with the project.OPTIONAL
    DateTime created_at,   // The time (ISO 8601 format) when the team was created.
    DateTime updated_at,   // The time (ISO 8601 format) when the team was last updated.
    string external_reference,   // External reference for the project.OPTIONAL
    string title,    // Title for the project.
    string description,  // Description for the project.
    decimal target_amount,   // The target amount for the project.
    decimal total_amount,     // The amount that was donated to the project (with decimals).
    int total_donations,     // The number of donations for the project.
    bool target_amount_achieved,     // Indicates whether the project target amount is achieved or not.
    bool visible,    // Indicates whether the project is visible or not.
    bool countable,  // Indicates whether the project is included in the webpages and counters of parent levels when project is closed.
    bool closed,     // Indicates whether the project is closed or not.
    bool ended,  // Indicates whether the project is ended or not.
    DateTime end_date,     // The countdown date (ISO 8601 format) for this project.OPTIONAL
    string url,  // The URL to the project.
    string donate_url   // The URL to directly make a donation to the project.
):IResource
{
    public IBihzResource Map()
    {
        return new BihzProject
        {
            AantalDonaties = this.total_donations,
            Beeindigd = this.ended,
            CreatieDatum = this.created_at,
            DoelBedrag = this.target_amount,
            DoelBedragBereikt = this.target_amount_achieved,
            DonatieUrl = this.donate_url,
            EindDatum = this.end_date,
            //EvenementId = this.SegmentId, //TODO: which Id????
            ExterneReferentie = this.external_reference,
            Gesloten = this.closed,
            Omschrijving = this.description,
            ProjectId = this.id,
            SiteId = this.site_id,
            Slug = this.slug,
            Titel = this.title,
            TotaalBedrag = this.total_amount,
            Url = this.url,
            WijzigDatum = this.updated_at,
            Zichtbaar = this.visible
        };
    }
}