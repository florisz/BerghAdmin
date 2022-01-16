using Newtonsoft.Json;

namespace BerghAdmin.Services.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class DonationPageResponse
{
    [JsonProperty(PropertyName = "total_entries")]
    public int TotalEntries { get; set; }

    [JsonProperty(PropertyName = "total_entries")]
    public int TotalPages { get; set; }

    [JsonProperty(PropertyName = "total_entries")]
    public int PerPage { get; set; }

    [JsonProperty(PropertyName = "total_entries")]
    public int CurrentPage { get; set; }

    [JsonProperty(PropertyName = "donations")]
    public DonationResponse[] Donations { get; set; }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
