using BerghAdmin.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.DbContexts;

public class ApplicationDbContext : IdentityUserContext<User, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Persoon> Personen { get; set; }
    public DbSet<User>  Gebruikers { get; set; }
    public DbSet<Rol> Rollen { get; set; }
    public DbSet<Document> Documenten { get; set; }
}
