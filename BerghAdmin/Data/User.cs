using Microsoft.AspNetCore.Identity;

namespace BerghAdmin.Data
{
    public class User
    {
        // reference to the persoon who is currently logged in as user
        // to be done: refactoren naar Microsoft.AspNetCore.Identity
        public Persoon CurrentUser { get; set; }

        public int Id { get; set; }
        public string Name{ get; set; }
        public string[] Roles { get; set; }
        public string EmailAddress { get; set;}
    }
}
