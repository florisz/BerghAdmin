
using System.Text.Json.Serialization;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Projects : Issues
{
    public override string Endpoint => "projects";

    [JsonPropertyName("total_entries")]
    public int TotalEntries { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }

    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("projects")]
    public Project[] ProjectArray { get; set; }

    public override IEnumerable<Project> GetIssues<Project>()
    {
        return (IEnumerable<Project>)ProjectArray.ToList();
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
