using BIHZ.AdminPortaal.Data;
using System.Collections.Generic;

namespace BIHZ.AdminPortaal.Services
{
    public interface IPersoonService
    {
        List<Persoon> GetPersonen();
        Persoon GetPersoonById(int id);
        void SavePersoon(Persoon persoon);
        void DeletePersoon(int id);
    }
}
