using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
#pragma warning disable IDE1006 // Naming Styles

public class ActionResponse
{
    public Action Action { get; set; }
}

public record Action(int id,
    string slug,
    int site_id,
    int segment_id,
    int project_id,
    int team_id,
    DateTime created_at,
    DateTime updated_at,
    Owner owner,
    string external_reference,
    bool team_captain,
    string first_name,
    string infix,
    string last_name,
    string avatar_url,
    bool fundraiser_page,
    string title,
    string description,
    decimal target_amount,
    decimal total_amount,
    int total_donations,
    bool target_amount_achieved,
    bool visible,
    bool countable,
    bool closed,
    bool ended,
    DateTime end_date,
    int previous_participations,
    string url,
    string donate_url) : Resource
{ 

    public BihzActie Map()
    {
        return new BihzActie
        {
            ActionId = this.id,
            ProjectId = this.project_id,
            Slug = this.slug,
            SiteId = this.site_id,
            UserId = this.owner.id,
            Email = this.owner.email,
            CreatieDatum = this.created_at,
            WijzigDatum = this.updated_at,
            ExterneReferentie = this.external_reference,
            Voornaam = this.first_name,
            Tussenvoegsels = this.infix,
            Achternaam = this.last_name,
            Titel = this.title,
            Omschrijving = this.description,
            DoelBedrag = this.target_amount,
            TotaalBedrag = this.total_amount,
            AantalDonaties = this.total_donations,
            DoelBedragBereikt = this.target_amount_achieved,
            Beeindigd = this.ended,
            EindDatum = this.end_date,
            Url = this.url,
            DoneerUrl = this.donate_url,
        };
    }

}

public record Owner
{
    public int id { get; set; }
    public string first_name { get; set; }
    public string infix { get; set; }
    public string last_name { get; set; }
    public string email { get; set; }
}