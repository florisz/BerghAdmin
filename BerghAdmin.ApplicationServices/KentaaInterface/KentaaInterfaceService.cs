using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.Options;

using System.Net.Http;
using System.Text.Json;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public class KentaaInterfaceService : IKentaaInterfaceService
{
    readonly HttpClient _httpClient;
    readonly KentaaSession _session;
    public KentaaInterfaceService(IOptions<KentaaConfiguration> settings, IHttpClientFactory factory)
    {
        _session = new KentaaSession(settings.Value.KentaaHost, settings.Value.KentaaBasePath, settings.Value.ApiKey);
        _httpClient = _session.Connect(factory);
    }

    public async Task<Donation> GetDonationById(int donationId)
    {
        var url = _session.Url($"donations/{donationId}");
        var donation = await GetKentaaResponse<DonationResponse>(url);
        return donation.data;
    }

    public async Task<IEnumerable<T>> GetKentaaIssuesByQuery<TList,T>(KentaaFilter filter) where TList : Issues 
    {
        var issues = new List<T>();

        var endpoint = GetEndpoint(typeof(TList));
        var url = _session.Url(endpoint, filter);
        var response = await GetKentaaResponse<TList>(url);
        var issueArray = response?.GetIssues<T>();

        while (issueArray != null && issueArray.Any())
        {
            issues.AddRange(issueArray);

            url = _session.Url(endpoint, filter.NextPage());
            response = await GetKentaaResponse<TList>(url);
            issueArray = response?.GetIssues<T>();
        }

        return issues;
    }

    private string GetEndpoint(Type type)
    {
        // kan dat niet anders??!
        return type.GetProperty("Endpoint").GetValue(null, null) as string;
    }

    private async Task<T> GetKentaaResponse<T>(string url)
    {
        var options = new JsonSerializerOptions
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
        };

        var response = await _httpClient.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var o = JsonSerializer.Deserialize<T>(json, options);
            if (o == null)
                throw new ApplicationException($"Could not deserialize JSON '{o}' into kentaa issue list");

            return o;
        }
    
        throw new ApplicationException($"Could not get {typeof(T).Name} donation from Kentaa; {url}");
    }


}