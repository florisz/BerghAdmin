using System.Text.Json.Serialization;

namespace BerghAdmin.Services.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class DonationResponse
{
    [JsonPropertyName("donation")]
    public Donation data { get; set; }

}

public class Donation
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }

    [JsonPropertyName("project_id")]
    public int ProjectId { get; set; } = 0;

    [JsonPropertyName("action_id")]
    public int ActionId { get; set; } = 0;

    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public string UpdatedAt { get; set; }

    [JsonPropertyName("first_name")]
    public string? FirstName { get; set; }

    [JsonPropertyName("infix")]
    public string? Infix { get; set; }

    [JsonPropertyName("last_name")]
    public string? LastName { get; set; }

    [JsonPropertyName("company")]
    public string? Company { get; set; }

    [JsonPropertyName("anonymous")]
    public bool Anonymous { get; set; } = false;

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("newsletter")]
    public bool NewsLetter { get; set; } = false;

    [JsonPropertyName("device_type")]
    public string? DeviceType { get; set; }

    [JsonPropertyName("frequency_type")]
    public string FrequencyType { get; set; }

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = "EUR";

    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [JsonPropertyName("transaction_costs")]
    public decimal? TransactionCost { get; set; }

    [JsonPropertyName("registration_fee")]
    public bool RegistrationFee { get; set; } = false;

    [JsonPropertyName("registration_fee_amount")]
    public decimal? RegistrationFeeAmount { get; set; }

    [JsonPropertyName("total_amount")]
    public decimal TotalAmount { get; set; }

    [JsonPropertyName("receivable_amount")]
    public decimal ReceivableAmount { get; set; }

    [JsonPropertyName("countable")]
    public bool Countable { get; set; }

    [JsonPropertyName("invoicenumber")]
    public string InvoiceNumber { get; set; }

    [JsonPropertyName("payment_method")]
    public string PaymentMethod { get; set; }

    [JsonPropertyName("payment_status")]
    public string PaymentStatus { get; set; }

    [JsonPropertyName("payment_status_at")]
    public string PaymentStatusAt { get; set; }

    [JsonPropertyName("transaction_id")]
    public string PaymentTransactionId { get; set; }

    [JsonPropertyName("payment_id")]
    public string PaymentId { get; set; }

    [JsonPropertyName("payment_description")]
    public string PaymentDescription { get; set; }

    [JsonPropertyName("account_iban")]
    public string? AccountIban { get; set; }

    [JsonPropertyName("account_bic")]
    public string? AccountBic { get; set; }

    [JsonPropertyName("account_name")]
    public string? AccountName { get; set; }

    [JsonPropertyName("target_url")]
    public string DonationTargetUrl { get; set; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
