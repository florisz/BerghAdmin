using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable IDE1006 // Naming Styles

public record DonationResponse(Donation donation);

public record Donation
(
    int id,
    int site_id,             // reference to Bergh in het Zadel
    int project_id,     // reference to Fietstocht
    int action_id,      // reference to the fietser's donation page
    DateTime created_at,
    DateTime updated_at,
    string? first_name,
    string? infix,
    string? last_name,
    string? company,
    bool anonymous,
    string? email,
    string? message,
    bool newsletter,
    string? device_type,
    string frequency_type,
    decimal amount,
    decimal? transaction_costs,
    bool registration_fee,
    decimal? registration_fee_amount,
    decimal total_amount,
    decimal receivable_amount,
    bool countable,
    string invoicenumber,
    string payment_method,
    string payment_status,
    DateTime? payment_status_at,
    string transaction_id,
    string payment_id,
    string payment_description,
    string? account_iban,
    string? account_bic,
    string? account_name,
    string target_url,
    string currency = "EUR"
) : IResource
{
    public BihzDonatie Map()
    {
        return new BihzDonatie
        {
            DonationId = this.id,
            ActionId = this.action_id,
            ProjectId = this.project_id,
            CreatieDatum = this.created_at,
            WijzigDatum = this.updated_at,
            DonatieBedrag = this.amount,
            TransactionKosten = this.transaction_costs,
            RegistratieFee = this.registration_fee,
            RegistratieFeeBedrag = this.registration_fee_amount,
            TotaalBedrag = this.total_amount,
            NettoBedrag = this.receivable_amount,
            Currency = GetCurrency(this.currency),
            BetaalStatus = GetBetaalStatus(this.payment_status),
            BetaalStatusOp = this.payment_status_at,
            BetaalTransactieId = this.transaction_id,
            BetaalId = this.payment_id,
            BetaalOmschrijving = this.payment_description,
            AccountIban = this.account_iban,
            AccountBic = this.account_bic,
        };
    }

    private static CurrencyCodeEnum GetCurrency(string specifier)
    {
        return specifier switch
        {
            "EUR" => CurrencyCodeEnum.Euro,
            "DKK" => CurrencyCodeEnum.DeenseKroon,
            "GBP" => CurrencyCodeEnum.BritsePond,
            "NOK" => CurrencyCodeEnum.NoorseKroon,
            "SEK" => CurrencyCodeEnum.ZweedseKroon,
            "USD" => CurrencyCodeEnum.AmerikaanseDollar,
            _ => CurrencyCodeEnum.Unknown,
        };
    }

    private static PaymentStatusEnum GetBetaalStatus(string paymentStatus)
    {
        return paymentStatus.ToLower() switch
        {
            "canceled" => PaymentStatusEnum.Canceled,
            "chargedback" => PaymentStatusEnum.Chargedback,
            "paid" => PaymentStatusEnum.Paid,
            "pledged" => PaymentStatusEnum.Pledged,
            "refunded" => PaymentStatusEnum.Refunded,
            "started" => PaymentStatusEnum.Started,
            _ => PaymentStatusEnum.Unknown,
        };
    }

}
