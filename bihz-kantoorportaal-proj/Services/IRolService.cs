using bihz.kantoorportaal.Data;
using System.Collections.Generic;

namespace bihz.kantoorportaal.Services
{
    public interface IRolService
    {
        List<Rol> GetRollen();
        Rol GetRolById(int id);
    }
}
