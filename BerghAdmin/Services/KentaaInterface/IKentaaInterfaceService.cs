using BerghAdmin.Services.KentaaInterface.KentaaModel;
using System.Threading.Tasks;

namespace BerghAdmin.Services.KentaaInterface;

public interface IKentaaInterfaceService
{
    Task<Donation> GetDonationById(int donationId);

    Task<IEnumerable<Donation>> GetDonationsByQuery(KentaaFilter filter);
}
