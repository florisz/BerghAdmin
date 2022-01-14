namespace BerghAdmin.Data;

public class KentaaDonatie
{
    public int Id { get; set; }
    public int KentaaId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool Anonymous { get; set; } = false;
    public string? FirstName { get; set; }
    public string? Infix { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Message { get; set; }
    public bool NewsLetter { get; set; } = false;
    public DeviceTypeEnum DeviceType { get; set; }
    public FrequencyTypeEnum FrequencyType { get; set; }
    public string CurrencyCode { get; set; } = "EUR";
    public CurrencyCodeEnum Currency { get; set; }
    public decimal Amount { get; set; }
    public decimal? RegistrationFeeAmount { get; set; }
    public decimal? TransactionCost { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal ReceivableAmount { get; set; }
    public bool Countable { get; set; }
    public bool StartDonation { get; set; }
    public bool RegistrationFee { get; set; }
    public string InvoiceNumber { get; set; }
    public string PaymentMethod { get; set; }
    public PaymentStatusEnum PaymentStatus { get; set; }
    public DateTime PaymentStatusAt { get; set; }
    public string PaymentTransactionId { get; set; }
    public string PaymentDescription { get; set; }
    public string AccountIban { get; set; }
    public string AccountBic { get; set; }
    public string AccountName { get; set; }
}

public enum DeviceTypeEnum
{
    Desktop,
    Tablet,
    Phone,
    Unknown
}

public enum FrequencyTypeEnum
{
    Oneoff,
    Monthly,
    Annually,
    Unknown
}

public enum CurrencyCodeEnum
{
    Euro,               // EUR
    DeenseKroon,        // DKK
    BritsePond,         // GBP
    NoorseKroon,        // NOK
    ZweedseKroon,       // SEK
    AmerikaanseDollar,  // USD
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
