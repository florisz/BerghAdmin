#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using Microsoft.AspNetCore.Identity;

using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Authorization;

// TO DO: find out if it is better to derive from IdentityUser<string>. Using a string as key might map better to
//        the UserManager and IdentityUser from the Microsoft implementation
public class User : IdentityUser<int>
{
    public int LoginCount { get; set; }

    public string Name { get; set; }
}
