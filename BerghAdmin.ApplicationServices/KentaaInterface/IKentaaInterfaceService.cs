using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public interface IKentaaInterfaceService
{
    Task<Donation> GetDonationById(int donationId);

    Task<IEnumerable<KentaaModel.Donation>> GetDonationsByQuery(KentaaFilter filter);
    Task<IEnumerable<KentaaModel.Action>> GetActionsByQuery(KentaaFilter filter);
    Task<IEnumerable<KentaaModel.Project>> GetProjectsByQuery(KentaaFilter filter);
    Task<IEnumerable<KentaaModel.User>> GetUsersByQuery(KentaaFilter filter);
}
