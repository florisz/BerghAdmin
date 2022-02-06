namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

#pragma warning disable IDE1006 // Naming Styles
public record Donations(int total_entries, int total_pages, int per_page, int current_page, Donation[] donations)
    : IResources<Donation>
{
    public IEnumerable<Donation> GetResources() => donations;
}
