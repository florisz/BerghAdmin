using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BerghAdmin.General;

public static class NavigationManagerExtensions
{
    public static string CreateUrlWithContext(this NavigationManager navigationManager, int id, string url)
    {
        var baseUri = navigationManager.BaseUri;
        var queryParameters = new Dictionary<string, string?> 
                                    { 
                                        { "Id", id.ToString() }, 
                                        { "ReturnUrl", navigationManager.Uri } 
                                    };
        return QueryHelpers.AddQueryString($"{baseUri}{url}", queryParameters);
    }
}
