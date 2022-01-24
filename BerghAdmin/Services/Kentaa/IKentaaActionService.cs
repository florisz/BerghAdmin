using KM=BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;
using BerghAdmin.General;

namespace BerghAdmin.Services.Kentaa;

public interface IKentaaActionService
{
    void AddKentaaAction(KM.Action kentaaAction);
    bool Exist(KentaaAction action);
    IEnumerable<KentaaAction>? GetAll();
    KentaaAction? GetById(int id);
    KentaaAction? GetByKentaaId(int kentaaId);
    ErrorCodeEnum Save(KentaaAction donatie);
}
