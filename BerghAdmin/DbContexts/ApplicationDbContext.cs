using BerghAdmin.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.DbContexts;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Persoon> Personen { get; set; }
    public DbSet<Rol> Rollen { get; set; }
    public DbSet<Document> Documenten { get; set; }
}
