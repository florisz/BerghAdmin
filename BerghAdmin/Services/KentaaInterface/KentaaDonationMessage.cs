using Newtonsoft.Json;

namespace BerghAdmin.Services.KentaaInterface;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class KentaaDonationMessage
{
    [JsonProperty(PropertyName = "id")]
    public int Id { get; set; }

    [JsonProperty(PropertyName = "site_id")]
    public int SiteId { get; set; }

    [JsonProperty(PropertyName = "created_at")]
    public string CreatedAt { get; set; }

    [JsonProperty(PropertyName = "updated_at")]
    public string UpdatedAt { get; set; }

    [JsonProperty(PropertyName = "first_name")]
    public string? FirstName { get; set; }

    [JsonProperty(PropertyName = "infix")]
    public string? Infix { get; set; }

    [JsonProperty(PropertyName = "last_name")]
    public string? LastName { get; set; }

    [JsonProperty(PropertyName = "company")]
    public string? Company { get; set; }

    [JsonProperty(PropertyName = "anonymous")]
    public bool Anonymous { get; set; } = false;

    [JsonProperty(PropertyName = "email")]
    public string? Email { get; set; }

    [JsonProperty(PropertyName = "message")]
    public string? Message { get; set; }

    [JsonProperty(PropertyName = "newsletter")]
    public bool NewsLetter { get; set; } = false;

    [JsonProperty(PropertyName = "device_type")]
    public string? DeviceType { get; set; }

    [JsonProperty(PropertyName = "frequency_type")]
    public string FrequencyType { get; set; }

    [JsonProperty(PropertyName = "currency")]
    public string Currency { get; set; } = "EUR";

    [JsonProperty(PropertyName = "amount")]
    public string Amount { get; set; }

    [JsonProperty(PropertyName = "transaction_costs")]
    public string? TransactionCost { get; set; }

    [JsonProperty(PropertyName = "registration_fee")]
    public bool RegistrationFee { get; set; } = false;

    [JsonProperty(PropertyName = "registration_fee_amount")]
    public string? RegistrationFeeAmount { get; set; }

    [JsonProperty(PropertyName = "total_amount")]
    public string TotalAmount { get; set; }

    [JsonProperty(PropertyName = "receivable_amount")]
    public string ReceivableAmount { get; set; }

    [JsonProperty(PropertyName = "countable")]
    public bool Countable { get; set; }

    [JsonProperty(PropertyName = "invoicenumber")]
    public string InvoiceNumber { get; set; }

    [JsonProperty(PropertyName = "payment_method")]
    public string PaymentMethod { get; set; }

    [JsonProperty(PropertyName = "payment_status")]
    public string PaymentStatus { get; set; }

    [JsonProperty(PropertyName = "payment_status_at")]
    public string PaymentStatusAt { get; set; }

    [JsonProperty(PropertyName = "transaction_id")]
    public string PaymentTransactionId { get; set; }

    [JsonProperty(PropertyName = "payment_id")]
    public string PaymentId { get; set; }

    [JsonProperty(PropertyName = "payment_description")]
    public string PaymentDescription { get; set; }

    [JsonProperty(PropertyName = "account_iban")]
    public string? AccountIban { get; set; }

    [JsonProperty(PropertyName = "account_bic")]
    public string? AccountBic { get; set; }

    [JsonProperty(PropertyName = "account_name")]
    public string? AccountName { get; set; }

    [JsonProperty(PropertyName = "target_url")]
    public string DonationTargetUrl { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
