namespace BerghAdmin.Data;

public class KentaaDonatie
{
    public int Id { get; set; }

    public int SiteId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
    public string? DonateurNaam{ get; set; }
    
    public bool Anoniem { get; set; } = false;
    
    public string? DonateurEmail { get; set; }
    
    public string? DonateurBericht { get; set; }
    
    public bool NieuwsbriefGewenst { get; set; } = false;
    
    public DeviceTypeEnum DeviceType { get; set; }
    
    public FrequencyTypeEnum FrequencyType { get; set; }
    
    public CurrencyCodeEnum Currency { get; set; } = CurrencyCodeEnum.Euro;
    
    public decimal Bedrag { get; set; }

    public decimal TransactionKosten { get; set; }

    public bool RegistrationFee { get; set; } = false;

    public decimal RegistrationFeeAmount { get; set; }

    public decimal TotaalBedrag { get; set; }
    
    public decimal NettoBedrag { get; set; }
    
    public bool Countable { get; set; }
    
    public string? BetaalFactuurNummer { get; set; }
    
    public string? BetaalMethode { get; set; }
    
    public PaymentStatusEnum BetaalStatus { get; set; }
    
    public DateTime BetaalStatusOp { get; set; }
    
    public string? BetaalTransactieId { get; set; }

    public string? BetaalId { get; set; }

    public string? BetaalOmschrijving { get; set; }

    public string? AccountIban { get; set; }
    
    public string? AccountBic { get; set; }

    public string? AccountName { get; set; }

    public string? DonatieUrl{ get; set; }
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
