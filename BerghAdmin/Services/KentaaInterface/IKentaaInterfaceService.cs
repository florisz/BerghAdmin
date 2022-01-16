using BerghAdmin.Services.KentaaInterface.KentaaModel;
using System.Threading.Tasks;

namespace BerghAdmin.Services.KentaaInterface;

public interface IKentaaInterfaceService
{
    Task<DonationResponse> GetDonationById(string donationId);

    Task<IEnumerable<DonationResponse>> GetIssuesByQuery(KentaaFilter filter);
}
