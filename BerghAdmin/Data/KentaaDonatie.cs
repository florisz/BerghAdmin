using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class KentaaDonatie : Donatie
{
    public KentaaDonatie()
    { }

    public KentaaDonatie(Donation kentaaDonatie)
    {
        KentaaDonationId = kentaaDonatie.Id;
        KentaaActionId = kentaaDonatie.ActionId;
        KentaaProjectId = kentaaDonatie.ProjectId;
        CreatieDatum = kentaaDonatie.CreatedAt;
        WijzigDatum = kentaaDonatie.UpdatedAt;
        DonatieBedrag = kentaaDonatie.Amount;
        TransactionKosten = kentaaDonatie.TransactionCost;
        RegistratieFee = kentaaDonatie.RegistrationFee;
        RegistratieFeeBedrag = kentaaDonatie.RegistrationFeeAmount;
        TotaalBedrag = kentaaDonatie.TotalAmount;
        NettoBedrag = kentaaDonatie.ReceivableAmount;
        Currency = GetCurrency(kentaaDonatie.Currency);
        BetaalStatus = GetBetaalStatus(kentaaDonatie.PaymentStatus);
        BetaalStatusOp = kentaaDonatie.PaymentStatusAt;
        BetaalTransactieId = kentaaDonatie.PaymentTransactionId;
        BetaalId = kentaaDonatie.PaymentId;
        BetaalOmschrijving = kentaaDonatie.PaymentDescription;
        AccountIban = kentaaDonatie.AccountIban;
        AccountBic = kentaaDonatie.AccountBic;
    }
    public int? KentaaDonationId { get; set; }
    public int? KentaaActionId { get; set; }
    public int? KentaaProjectId { get; set; }
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


