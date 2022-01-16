using BerghAdmin.Services.Configuration;
using BerghAdmin.Services.KentaaInterface.KentaaModel;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace BerghAdmin.Services.KentaaInterface;

public class KentaaInterfaceService : IKentaaInterfaceService
{
    readonly IKentaaClient _kentaaClient;

    public KentaaInterfaceService(IOptions<KentaaConfiguration> settings)
    {
        var kentaaSession = new KentaaSession(settings.Value.ServerUrl, settings.Value.ApiKey);
        _kentaaClient = kentaaSession.Connect();
    }

    public async Task<DonationResponse> GetDonationById(string donationId)
    {
        return await _kentaaClient.GetDonationById(donationId);
    }

    public async Task<IEnumerable<DonationResponse>> GetIssuesByQuery(KentaaFilter filter)
    {
        var kentaaIssues = new List<DonationResponse>();
        var response = await _kentaaClient.GetDonationMessages(filter.Build());
        kentaaIssues.AddRange(response.Donations);

        //if (response.StartAt + filter.PageSize >= response.Total) return kentaaIssues;

        //do
        //{
        //    filter = filter.NextPage();
        //    response = await _kentaaClient.GetDonationMessages((filter.Build()));
        //    kentaaIssues.AddRange(response.Issues);
        //} while (response.StartAt + filter.PageSize < response.Total);

        return kentaaIssues;
    }
}
