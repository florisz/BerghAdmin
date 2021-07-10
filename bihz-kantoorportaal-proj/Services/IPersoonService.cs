using bihz.kantoorportaal.Data;
using System.Collections.Generic;

namespace bihz.kantoorportaal.Services
{
    public interface IPersoonService
    {
        List<Persoon> GetPersonen();
        Persoon GetPersoonById(int id);
        void SavePersoon(Persoon persoon);
        void DeletePersoon(int id);
    }
}
