using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Data;

public class KentaaDonation
{
    public KentaaDonation()
    { }

    public KentaaDonation(Donation donation)
    {
        Update(donation);
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

    public void Update(Donation donation)
    {
        DonationId = donation.Id;
        ActionId = donation.ActionId;
        ProjectId = donation.ProjectId;
        CreatieDatum = donation.CreatedAt;
        WijzigDatum = donation.UpdatedAt;
        DonatieBedrag = donation.Amount;
        TransactionKosten = donation.TransactionCost;
        RegistratieFee = donation.RegistrationFee;
        RegistratieFeeBedrag = donation.RegistrationFeeAmount;
        TotaalBedrag = donation.TotalAmount;
        NettoBedrag = donation.ReceivableAmount;
        Currency = GetCurrency(donation.Currency);
        BetaalStatus = GetBetaalStatus(donation.PaymentStatus);
        BetaalStatusOp = donation.PaymentStatusAt;
        BetaalTransactieId = donation.PaymentTransactionId;
        BetaalId = donation.PaymentId;
        BetaalOmschrijving = donation.PaymentDescription;
        AccountIban = donation.AccountIban;
        AccountBic = donation.AccountBic;
    }

    private static CurrencyCodeEnum GetCurrency(string specifier)
    {
        if (specifier == null)
        {
            return CurrencyCodeEnum.Unknown;
        }
        switch (specifier)
        {
            case "EUR":
                return CurrencyCodeEnum.Euro;
            case "DKK":
                return CurrencyCodeEnum.DeenseKroon;
            case "GBP":
                return CurrencyCodeEnum.BritsePond;
            case "NOK":
                return CurrencyCodeEnum.NoorseKroon;
            case "SEK":
                return CurrencyCodeEnum.ZweedseKroon;
            case "USD":
                return CurrencyCodeEnum.AmerikaanseDollar;
        }

        return CurrencyCodeEnum.Euro;
    }
    private PaymentStatusEnum GetBetaalStatus(string paymentStatus)
    {
        if (paymentStatus == null)
        {
            return PaymentStatusEnum.Unknown;
        }
        switch (paymentStatus.ToLower())
        {
            case "canceled":
                return PaymentStatusEnum.Canceled;
            case "chargedback":
                return PaymentStatusEnum.Chargedback;
            case "paid":
                return PaymentStatusEnum.Paid;
            case "pledged":
                return PaymentStatusEnum.Pledged;
            case "refunded":
                return PaymentStatusEnum.Refunded;
            case "started":
                return PaymentStatusEnum.Started;
        }

        return PaymentStatusEnum.Unknown;
    }


}

public enum CurrencyCodeEnum
{
    Euro,               // EUR
    DeenseKroon,        // DKK
    BritsePond,         // GBP
    NoorseKroon,        // NOK
    ZweedseKroon,       // SEK
    AmerikaanseDollar,  // USD
    Unknown

}

public enum PaymentStatusEnum
{
    Pledged,
    Refunded,
    Chargedback,
    Canceled,
    Paid,
    Started,
    Unknown
}


