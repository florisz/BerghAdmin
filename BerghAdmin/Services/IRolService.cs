using BerghAdmin.Data;
using System.Collections.Generic;

namespace BerghAdmin.Services
{
    public interface IRolService
    {
        List<Rol> GetRollen();
        Rol GetRolById(RolTypeEnum id);
    }
}
