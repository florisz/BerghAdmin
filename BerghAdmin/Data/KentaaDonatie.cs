using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Data;

public class KentaaDonatie : Donatie
{
    public KentaaDonatie(/* Donation kentaaDonatie */)
    {

    }
    public int KentaaDonationId { get; set; }
    public int? KentaaActionId { get; set; }
    public int? KentaaProjectId { get; set; }
    public DateTime Datum { get; set; }
    public decimal Bedrag { get; set; }
    public decimal TransactionKosten { get; set; }
    public bool RegistrationFee { get; set; } = false;
    public decimal RegistrationFeeAmount { get; set; }
    public decimal TotaalBedrag { get; set; }
    public decimal NettoBedrag { get; set; }
    public CurrencyCodeEnum Currency { get; set; } = CurrencyCodeEnum.Euro;
    public PaymentStatusEnum BetaalStatus { get; set; }
    public DateTime BetaalStatusOp { get; set; }
    public string? BetaalTransactieId { get; set; }
    public string? BetaalId { get; set; }
    public string? BetaalOmschrijving { get; set; }
    public string? AccountIban { get; set; }
    public string? AccountBic { get; set; }
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


