
using System.Text.Json.Serialization;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Users : Issues
{
    public override string Endpoint => "users";

    [JsonPropertyName("total_entries")]
    public int TotalEntries { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }

    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("users")]
    public User[] UserArray { get; set; }

    public override IEnumerable<User> GetIssues<User>()
    {
        return (IEnumerable<User>)UserArray.ToList();
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
