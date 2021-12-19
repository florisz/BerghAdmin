#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;
public class User:IdentityUser<int>
{
    // reference to the persoon who is currently logged in as user
    public Persoon CurrentUser { get; set; }

    public string? Name { get; set; }
    [NotMapped]
    public string[] Roles { get; set; }

}
