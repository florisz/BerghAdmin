using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public interface IKentaaInterfaceService
{
    Task<Donation> GetDonationById(int donationId);

    Task<IEnumerable<T>> GetKentaaIssuesByQuery<TList, T>(KentaaFilter filter) where TList : Issues;
}
