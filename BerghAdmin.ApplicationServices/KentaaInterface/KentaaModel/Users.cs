
using System.Text.Json.Serialization;

namespace BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

public record Users(int total_entries, int total_pages, int per_page, int current_page, User[] users)
    : IResources<User>
{
    public IEnumerable<User> GetResources() => users;
}