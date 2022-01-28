using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public interface IKentaaInterfaceService
{
    Task<Donation> GetDonationById(int donationId);
    IAsyncEnumerable<T> GetKentaaIssuesByQuery<TList, T>(KentaaFilter filter)
        where TList : Issues<T>, new()
        where T : Issue;

    //IAsyncEnumerable<T> GetKentaaIssuesByQuery<T>(KentaaFilter filter) where T : Issue, new();
}
