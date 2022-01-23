using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using System.ComponentModel.DataAnnotations.Schema;

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
    public int KentaaActionId { get; set; }
    public string? KentaaSlug { get; set; }
    public int? SiteId { get; set; }
    public int? KentaaProjectId { get; set; }
    public DateTime CreatieDatum { get; set; }
    public DateTime WijzigDatum { get; set; }
    public string? ExterneReferentie { get; set; }
    public string? Voornaam { get; set; }
    public string? Tussenvoegsels { get; set; }
    public string? Achternaam { get; set; }
    public string? Titel { get; set; }
    public string? Omschrijving { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal DoelBedrag { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal TotaalBedrag { get; set; }
    public int AantalDonaties { get; set; }
    public bool DoelBedragBereikt { get; set; }
    public bool Beeindigd { get; set; }
    public DateTime EindDatum { get; set; }
    public string? Url { get; set; }
    public string? DoneerUrl { get; set; }

    public void Update(ApplicationServices.KentaaInterface.KentaaModel.Action action)
    {
        KentaaActionId = action.Id;
        KentaaProjectId = action.ProjectId;
        KentaaSlug = action.Slug;
        SiteId = action.SiteId;
        KentaaProjectId = action.ProjectId;
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

