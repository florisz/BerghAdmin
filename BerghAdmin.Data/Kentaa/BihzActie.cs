﻿namespace BerghAdmin.Data.Kentaa;

public class BihzActie
{
    public BihzActie()
    {
    }

    public BihzActie(BihzActie newActie)
    {
        this.UpdateFrom(newActie);
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
    public string? Email { get; set; }
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
    public int? PersoonId { get; set; }     // id to reference the corresponding Persoon in the BerghAdmin context

    public BihzActie UpdateFrom(BihzActie a)
    {
        ActionId = a.ActionId;
        SiteId = a.SiteId;
        ProjectId = a.ProjectId;
        UserId = a.UserId;
        Slug = a.Slug;
        CreatieDatum = a.CreatieDatum;
        WijzigDatum = a.WijzigDatum;
        ExterneReferentie = a.ExterneReferentie;
        Voornaam = a.Voornaam;
        Tussenvoegsels = a.Tussenvoegsels;
        Achternaam = a.Achternaam;
        Email = a.Email;
        Titel = a.Titel;
        Omschrijving = a.Omschrijving;
        DoelBedrag = a.DoelBedrag;
        TotaalBedrag = a.TotaalBedrag;
        AantalDonaties = a.AantalDonaties;
        DoelBedragBereikt = a.DoelBedragBereikt;
        Beeindigd = a.Beeindigd;
        EindDatum = a.EindDatum;
        Url = a.Url;
        DoneerUrl = a.DoneerUrl;
        PersoonId = a.PersoonId;

        return this;
    }
}

