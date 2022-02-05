namespace BerghAdmin.Data.Kentaa;

public class BihzDonatie : IBihzResource
{
    public BihzDonatie()
    {
    }

    public BihzDonatie(BihzDonatie donatie)
    {
        this.UpdateFrom(donatie);
    }

    public int Id { get; set; }
    public int DonationId { get; set; }
    public int ActionId { get; set; }
    public int ProjectId { get; set; }
    public DateTime CreatieDatum { get; set; }
    public DateTime WijzigDatum { get; set; }
    public decimal DonatieBedrag { get; set; }
    public decimal? TransactionKosten { get; set; }
    public bool RegistratieFee { get; set; } = false;
    public decimal? RegistratieFeeBedrag { get; set; }
    public decimal TotaalBedrag { get; set; }
    public decimal NettoBedrag { get; set; }
    public CurrencyCodeEnum Currency { get; set; } = CurrencyCodeEnum.Euro;
    public PaymentStatusEnum BetaalStatus { get; set; }
    public DateTime? BetaalStatusOp { get; set; }
    public string? BetaalTransactieId { get; set; }
    public string? BetaalId { get; set; }
    public string? BetaalOmschrijving { get; set; }
    public string? AccountIban { get; set; }
    public string? AccountBic { get; set; }
    public int? PersoonId { get; set; }     // id to reference the corresponding Persoon in the BerghAdmin context

    public BihzDonatie UpdateFrom(BihzDonatie d)
    {
        DonationId = d.DonationId;
        ActionId = d.ActionId;
        ProjectId = d.ProjectId;
        CreatieDatum = d.CreatieDatum;
        WijzigDatum = d.WijzigDatum;
        DonatieBedrag = d.DonatieBedrag;
        TransactionKosten = d.TransactionKosten;
        RegistratieFee = d.RegistratieFee;
        RegistratieFeeBedrag = d.RegistratieFeeBedrag;
        TotaalBedrag = d.TotaalBedrag;
        NettoBedrag = d.NettoBedrag;
        Currency = d.Currency;
        BetaalStatus = d.BetaalStatus;
        BetaalStatusOp = d.BetaalStatusOp;
        BetaalTransactieId = d.BetaalTransactieId;
        BetaalId = d.BetaalId;
        BetaalOmschrijving = d.BetaalOmschrijving;
        AccountIban = d.AccountIban;
        AccountBic = d.AccountBic;
        PersoonId = d.PersoonId;

        return this;
    }
}

public enum CurrencyCodeEnum
{
    Euro = 1,               // EUR
    DeenseKroon = 2,        // DKK
    BritsePond = 3,         // GBP
    NoorseKroon = 4,        // NOK
    ZweedseKroon = 5,       // SEK
    AmerikaanseDollar = 6,  // USD
    Unknown = 7

}

public enum PaymentStatusEnum
{
    Pledged = 1,
    Refunded = 2,
    Chargedback = 3,
    Canceled = 4,
    Paid = 5,
    Started = 6,
    Unknown = 7
}
