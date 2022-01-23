using BerghAdmin.General;
using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.Services.Donaties
{
    public interface IKentaaService
    {
        void ProcessKentaaAction(KM.@Action kentaaAction);
        void ProcessKentaaDonation(KM.Donation kentaaDonation);
        void ProcessKentaaProject(KM.Project kentaaProject);
        void ProcessKentaaUser(KM.User kentaaUser);
    }
}
