
using System.Text.Json.Serialization;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

public class Actions : Resources<Action>
{
    public override string Endpoint => "actions";

    [JsonPropertyName("total_entries")]
    public int TotalEntries { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }

    [JsonPropertyName("current_page")]
    public int CurrentPage { get; set; }

    [JsonPropertyName("actions")]
    public Action[] ActionArray { get; set; }

    public override IEnumerable<Action> GetResources()
    {
        return ActionArray;
    }
}

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
