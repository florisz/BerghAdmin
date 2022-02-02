namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable IDE1006 // Naming Styles

public record Actions(int total_entries, int total_pages, int per_page, int current_page, Action[] actions) 
    :IResources<Action>
{ 
    public IEnumerable<Action> GetResources() => actions;
}