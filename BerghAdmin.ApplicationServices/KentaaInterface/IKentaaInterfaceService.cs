using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.ApplicationServices.KentaaInterface;

public interface IKentaaInterfaceService
{
    Task<Donation> GetDonationById(int donationId);
    IAsyncEnumerable<T> GetKentaaResourcesByQuery<TList, T>(KentaaFilter filter)
        where TList : IResources<T>
        where T : IResource;
}
