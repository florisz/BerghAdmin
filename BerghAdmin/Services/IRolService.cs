namespace BerghAdmin.Services;

public interface IRolService
{
    List<Rol> GetRollen();
    Rol GetRolById(RolTypeEnum id);
    Task AddRol(Rol rol);
    RolListItem[] GetAlleRolListItems();
}
