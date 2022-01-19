using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.Services.KentaaInterface;

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
        _session = new KentaaSession(settings.Value.KentaaUrl, settings.Value.ApiKey);
        _httpClient = _session.Connect(factory);
    }

    public async Task<Donation> GetDonationById(int donationId)
    {
        var url = $"{_session.Url}/donations/{donationId}?api_key={_session.ApiKey}";
        var donation = await GetKentaaResponse<DonationResponse>(url);
        return donation.data;
    }

    public async Task<IEnumerable<Donation>> GetDonationsByQuery(KentaaFilter filter)
    {
        var kentaaIssues = new List<Donation>();

        var url = $"{_session.Url}/donations?{filter.Build()};api_key={_session.ApiKey}";
        var donations = await GetKentaaResponse<Donations>(url);

        while (donations.DonationArray.Any())
        {
            kentaaIssues.AddRange(donations.DonationArray);

            filter = filter.NextPage();
            url = $"{_session.Url}/donations?{filter.Build()};api_key={_session.ApiKey}";
            donations = await GetKentaaResponse<Donations>(url);
        }

        return kentaaIssues;
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
                throw new ApplicationException($"Could not deserialize JSON '{o}' into donation list");

            return o;
        }
    
        throw new ApplicationException($"Could not get {typeof(T).Name} donation from Kentaa; {url}");
    }
}