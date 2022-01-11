using BerghAdmin.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.DbContexts;

public class ApplicationDbContext : IdentityUserContext<User, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Persoon>? Personen { get; set; }
    public DbSet<User>?  Gebruikers { get; set; }
    public DbSet<Rol>? Rollen { get; set; }
    public DbSet<Document>? Documenten { get; set; }
    
    public DbSet<Factuur>? Facturen { get; set; }
    public DbSet<Betaling>? Betalingen { get; set; }
    public DbSet<VerzondenMail>? VerzondenMails { get; set; }
    public DbSet<Organisatie>? Organisaties { get; set; }
    public DbSet<Donatie>? Donaties { get; set; }
    public DbSet<Evenement>? Evenementen{ get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder
            .Entity<Persoon>()
            .HasMany(p => p.Geadresseerden)
            .WithMany(m => m.Geadresseerden)
            .UsingEntity(j => j.ToTable("MailGeadresseerden"));
            
        modelBuilder
            .Entity<Persoon>()
            .HasMany(p => p.ccGeadresseerden)
            .WithMany(m => m.ccGeadresseerden)
            .UsingEntity(j => j.ToTable("MailccGeadresseerden"));
            
        modelBuilder
            .Entity<Persoon>()
            .HasMany(p => p.bccGeadresseerden)
            .WithMany(m => m.bccGeadresseerden)
            .UsingEntity(j => j.ToTable("MailbccGeadresseerden"));

        modelBuilder
            .Entity<FietsTocht>().ToTable("FietsTocht");

        modelBuilder
            .Entity<GolfDag>().ToTable("GolfDag");
    }

}
