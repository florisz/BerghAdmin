using System.Threading.Tasks;
using System.Net.Http.Headers;
using RestEase;
using BerghAdmin.Services.KentaaInterface.KentaaModel;

namespace BerghAdmin.Services.KentaaInterface;

/*  Query parameters     
    Parameter	    Description
    page	        Page number to show. Default is 1.
    per_page	    Number of results to return per page. Default is 25.
    created_after	Show results created after the 
                    given ISO 8601 datetime, ordered by created_at.
    updated_after	Show results updated after the 
                    given ISO 8601 datetime, ordered by updated_at.
    created_before	Show results created before the 
                    given ISO 8601 datetime, ordered by created_at.
    updated_before	Show results updated before the 
                    given ISO 8601 datetime, ordered by updated_at.
    */
public interface IKentaaClient
{
    [Get("donations/{id}?api_key=")]
    Task<DonationResponse> GetDonationById([Path] string id);

    [Get("donations?api_key=")]
    Task<DonationPageResponse> GetDonationMessages([RawQueryString] string kentaaQuery);

}
