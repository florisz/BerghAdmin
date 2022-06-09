#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Authorization;
public class User : IdentityUser<int>
{
    // reference to the persoon who is currently logged in as user
    public int? CurrentPersoonId { get; set; }
    public int LoginCount { get; set; }

    public string? Name { get; set; }
}
