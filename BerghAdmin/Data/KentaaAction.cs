using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Data;

public class KentaaAction
{
    public KentaaAction()
    { }

    public KentaaAction(ApplicationServices.KentaaInterface.KentaaModel.Action action)
    {
        Update(action);
    }
    
    public int Id { get; set; }
    public int ActionId { get; set; }
    public int? SiteId { get; set; }
    public int? ProjectId { get; set; }
    public int? UserId { get; set; }
    public string? Slug { get; set; }
    public DateTime CreatieDatum { get; set; }
    public DateTime WijzigDatum { get; set; }
    public string? ExterneReferentie { get; set; }
    public string? Voornaam { get; set; }
    public string? Tussenvoegsels { get; set; }
    public string? Achternaam { get; set; }
    public string? Titel { get; set; }
    public string? Omschrijving { get; set; }
    public decimal DoelBedrag { get; set; }
    public decimal TotaalBedrag { get; set; }
    public int AantalDonaties { get; set; }
    public bool DoelBedragBereikt { get; set; }
    public bool Beeindigd { get; set; }
    public DateTime EindDatum { get; set; }
    public string? Url { get; set; }
    public string? DoneerUrl { get; set; }

    public void Update(ApplicationServices.KentaaInterface.KentaaModel.Action action)
    {
        ActionId = action.Id;
        ProjectId = action.ProjectId;
        Slug = action.Slug;
        SiteId = action.SiteId;
        UserId = action.Owner.Id;
        CreatieDatum = action.CreatedAt;
        WijzigDatum = action.UpdatedAt;
        ExterneReferentie = action.ExternalReference;
        Voornaam = action.FirstName;
        Tussenvoegsels = action.Infix;
        Achternaam = action.LastName;
        Titel = action.Title;
        Omschrijving = action.Description;
        DoelBedrag = action.TargetAmount;
        TotaalBedrag = action.TotalAmount;
        AantalDonaties = action.TotalDonations;
        DoelBedragBereikt = action.TargetAmountAchieved;
        Beeindigd = action.Ended;
        EindDatum = action.EndDate;
        Url = action.Url;
        DoneerUrl = action.DonateUrl;
    }
}

