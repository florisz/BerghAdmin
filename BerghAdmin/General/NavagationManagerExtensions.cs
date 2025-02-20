using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BerghAdmin.General;

public static class NavigationManagerExtensions
{
    public static string CreateUrlWithSelectedIdAndReturnUrl(this NavigationManager navigationManager, int id, string url)
    {
        // get path without query parameters
        var baseUri = navigationManager.BaseUri;
        Uri uri = new Uri(navigationManager.Uri);

        // zeer lelijk maar voor nu ff goed genoeg
        // skip the first / in the path to get a correct url
        var returnUrl = $"{baseUri}{uri.AbsolutePath.Substring(1)}";

        var queryParameters = new Dictionary<string, string?>
                                    {
                                        { "Id", id.ToString() },
                                        { "ReturnUrl", returnUrl }
                                    };
        return QueryHelpers.AddQueryString($"{baseUri}{url}", queryParameters);
    }

    public static string CreateUrlWithSelectedId(this NavigationManager navigationManager, int id, string returnUrl)
    {
        var queryParameters = new Dictionary<string, string?>
                                    {
                                        { "SelectedId", id.ToString() }
                                    };
        return QueryHelpers.AddQueryString($"{returnUrl}", queryParameters);
    }
}
