using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class DonationResponse
{
    [JsonPropertyName("donation")]
    public Donation Data { get; set; }

}

public class Donation : Resource
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }             // reference to Bergh in het Zadel

    [JsonPropertyName("project_id")]            
    public int ProjectId { get; set; } = 0;     // reference to Fietstocht

    [JsonPropertyName("action_id")]
    public int ActionId { get; set; } = 0;      // reference to the fietser's donation page

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }

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

    [Column(TypeName = "decimal(18,2)")]
    [JsonPropertyName("amount")]
    public decimal Amount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [JsonPropertyName("transaction_costs")]
    public decimal? TransactionCost { get; set; }

    [JsonPropertyName("registration_fee")]
    public bool RegistrationFee { get; set; } = false;

    [Column(TypeName = "decimal(18,2)")]
    [JsonPropertyName("registration_fee_amount")]
    public decimal? RegistrationFeeAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [JsonPropertyName("total_amount")]
    public decimal TotalAmount { get; set; }

    [Column(TypeName = "decimal(18,2)")]
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
    public DateTime? PaymentStatusAt { get; set; }

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
