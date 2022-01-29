using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

using Microsoft.Extensions.Options;

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
        return donation.Data;
    }

    public async IAsyncEnumerable<T> GetKentaaResourcesByQuery<TList, T>(KentaaFilter filter) 
        where TList: Resources<T>, new()
        where T: Resource
    {
        var endpoint = new TList().Endpoint;
        var url = _session.Url(endpoint, filter);
        var issues = (await GetKentaaResponse<TList>(url)).GetIssues();

        while (issues.Any())
        {
            foreach (var issue in issues)
            {
                yield return issue;
            }

            filter = filter.NextPage();
            url = _session.Url(endpoint, filter);
            issues = (await GetKentaaResponse<TList>(url)).GetIssues();
        }
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