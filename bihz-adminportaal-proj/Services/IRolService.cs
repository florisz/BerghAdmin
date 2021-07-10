using BIHZ.AdminPortaal.Data;
using System.Collections.Generic;

namespace BIHZ.AdminPortaal.Services
{
    public interface IRolService
    {
        List<Rol> GetRollen();
        Rol GetRolById(int id);
    }
}
