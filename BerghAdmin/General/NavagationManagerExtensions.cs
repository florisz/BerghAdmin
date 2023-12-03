using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BerghAdmin.General;

public static class NavigationManagerExtensions
{
    public static string CreateUrlEditPersoon(this NavigationManager navigationManager, int persoonId)
    {
        var id = persoonId.ToString();
        var baseUri = navigationManager.BaseUri;
        var queryParameters = new Dictionary<string, string?> 
                                    { 
                                        { "PersoonId", id }, 
                                        { "ReturnUrl", navigationManager.Uri } 
                                    };
        return QueryHelpers.AddQueryString($"{baseUri}Personen/EditPersoon", queryParameters);
    }
}
