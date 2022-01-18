using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.KentaaInterface.KentaaModel;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Threading.Tasks;

namespace BerghAdmin.Services.KentaaInterface;

public class KentaaInterfaceService : IKentaaInterfaceService
{
    readonly HttpClient _httpClient;
    readonly KentaaSession _session;
    public KentaaInterfaceService(IOptions<KentaaConfiguration> settings, IHttpClientFactory factory)
    {
        _session = new KentaaSession(settings.Value.KentaaUrl, settings.Value.ApiKey);
        _httpClient = _session.Connect(factory);
    }

    public async Task<Donation?> GetDonationById(int donationId)
    {
        try
        {
            var streamTask = _httpClient.GetStreamAsync($"{_session.Url}/donations/{donationId}?api_key={_session.ApiKey}");

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };

            var donation = await JsonSerializer.DeserializeAsync<DonationResponse>(await streamTask, options);
            
            return donation?.data;
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Json deserialize error", ex);
        }
    }

    public async Task<IEnumerable<Donation>> GetDonationsByQuery(KentaaFilter filter)
    {
        var kentaaIssues = new List<Donation>();

        try
        {
            var url = $"{_session.Url}/donations?{filter.Build()};api_key={_session.ApiKey}";
            var streamTask = _httpClient.GetStreamAsync(url);

            var options = new JsonSerializerOptions
            {
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true,
                NumberHandling = System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString
            };
            while (true)
            {
                var donations = await JsonSerializer.DeserializeAsync<Donations>(await streamTask, options);

                if (donations?.DonationArray.Length == 0)
                {
                    break;
                }

                if (donations != null)
                {
                    kentaaIssues.AddRange(donations.DonationArray);
                }

                filter = filter.NextPage();
                url = $"{_session.Url}/donations?{filter.Build()};api_key={_session.ApiKey}";
                streamTask = _httpClient.GetStreamAsync(url);
            }
        }
        catch (Exception ex)
        {
            throw new ApplicationException("Json deserialize error", ex);
        }

        return kentaaIssues;
    }
}
