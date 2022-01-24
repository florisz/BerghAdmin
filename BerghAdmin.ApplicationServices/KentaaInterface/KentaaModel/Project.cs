using System.Text.Json.Serialization;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class ProjectResponse
{
    [JsonPropertyName("Action")]
    public Project data { get; set; }

}

public class Project 
{
    [JsonPropertyName("id")]
    public int Id { get; set; }  // Unique identifier for this project.

    [JsonPropertyName("slug")]
    public string Slug { get; set; }     // Unique identifier for this project, used to create a nice project URL.

    [JsonPropertyName("site_id")]
    public int SiteId { get; set; }     // Unique identifier for the site associated with the project.

    [JsonPropertyName("segment_id")]
    public int SegmentId { get; set; }  // Unique identifier for the segment associated with the project.OPTIONAL

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }   // The time (ISO 8601 format) when the team was created.

    [JsonPropertyName("updated_at")]
    public DateTime UpdatedAt { get; set; }   // The time (ISO 8601 format) when the team was last updated.

    [JsonPropertyName("external_reference")]
    public string ExternalReference { get; set; }   // External reference for the project.OPTIONAL

    [JsonPropertyName("title")]
    public string Title { get; set; }    // Title for the project.

    [JsonPropertyName("description")]
    public string Description { get; set; }  // Description for the project.

    [JsonPropertyName("target_amount")]
    public decimal TargetAmount { get; set; }   // The target amount for the project.

    [JsonPropertyName("total_amount")]
    public decimal TotalAmount { get; set; }     // The amount that was donated to the project (with decimals).

    [JsonPropertyName("total_donations")]
    public int TotalDonations { get; set; }     // The number of donations for the project.

    [JsonPropertyName("target_amount_achieved")]
    public bool TargetAmountAchieved { get; set; }     // Indicates whether the project target amount is achieved or not.

    [JsonPropertyName("visible")]
    public bool Visible { get; set; }    // Indicates whether the project is visible or not.

    [JsonPropertyName("countable")]
    public bool Countable { get; set; }  // Indicates whether the project is included in the webpages and counters of parent levels when project is closed.

    [JsonPropertyName("closed")]
    public bool Closed { get; set; }     // Indicates whether the project is closed or not.

    [JsonPropertyName("ended")]
    public bool Ended { get; set; }  // Indicates whether the project is ended or not.

    [JsonPropertyName("end_date")]
    public DateTime EndDate { get; set; }     // The countdown date (ISO 8601 format) for this project.OPTIONAL

    [JsonPropertyName("url")]
    public string Url { get; set; }  // The URL to the project.

    [JsonPropertyName("donate_url")]
    public string DonateUrl { get; set; }	 // The URL to directly make a donation to the project.
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
