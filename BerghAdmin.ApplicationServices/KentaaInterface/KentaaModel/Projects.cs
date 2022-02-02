namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable IDE1006 // Naming Styles
public record Projects(int total_entries, int total_pages, int per_page, int current_page, Project[] projects)
    : IResources<Project>
{ 
    public IEnumerable<Project> GetResources() => projects;
}
