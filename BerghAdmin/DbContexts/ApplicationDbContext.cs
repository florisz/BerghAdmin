using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BerghAdmin.Data.Kentaa;
using BerghAdmin.Authorization;
using System.Diagnostics;

namespace BerghAdmin.DbContexts;

public class ApplicationDbContext : IdentityUserContext<User, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }
    public DbSet<Persoon>? Personen { get; set; }
    public DbSet<Rol>? Rollen { get; set; }
    public DbSet<Document>? Documenten { get; set; }
    public DbSet<Factuur>? Facturen { get; set; }
    public DbSet<Betaling>? Betalingen { get; set; }
    public DbSet<VerzondenMail>? VerzondenMails { get; set; }
    public DbSet<Organisatie>? Organisaties { get; set; }
    public DbSet<Donatie>? Donaties { get; set; }
    public DbSet<Evenement>? Evenementen{ get; set; }
    public DbSet<BihzActie>? BihzActies { get; set; }
    public DbSet<BihzDonatie>? BihzDonaties { get; set; }
    public DbSet<BihzProject>? BihzProjects { get; set; }
    public DbSet<BihzUser>? BihzUsers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Persoon
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
            .Entity<Persoon>()
            .HasIndex(p => new { p.IsVerwijderd, p.EmailAdres })
            .IsUnique();
        modelBuilder
            .Entity<Persoon>()
            .HasMany(r => r.Rollen)
            .WithMany(p => p.Personen);
        // Persoon

        modelBuilder
            .Entity<FietsTocht>()
            .ToTable("FietsTochten");

        modelBuilder
            .Entity<Evenement>()
            .HasMany(f => f.Deelnemers)
            .WithMany(p => p.FietsTochten);

/*        modelBuilder
            .Entity<Evenement>()
            .HasMany(g => g.Deelnemers)
            .WithMany(p => p.GolfDagen);
*/
        modelBuilder
            .Entity<GolfDag>()
            .ToTable("GolfDagen");

        modelBuilder
            .Entity<Donatie>()
            .Property(p => p.Bedrag).HasPrecision(18, 2);

        modelBuilder
            .Entity<Betaling>()
            .Property(p => p.Bedrag).HasPrecision(18, 2);

        modelBuilder
            .Entity<BihzUser>()
            .HasIndex(u => u.UserId)
            .IsUnique();

        // BihzDonatie
        modelBuilder
            .Entity<BihzDonatie>()
            .ToTable("BihzDonaties");
        modelBuilder
            .Entity<BihzDonatie>()
            .Property(p => p.DonatieBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzDonatie>()
            .Property(p => p.TransactionKosten).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzDonatie>()
            .Property(p => p.RegistratieFeeBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzDonatie>()
            .Property(p => p.TotaalBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzDonatie>()
            .Property(p => p.NettoBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzDonatie>()
            .HasIndex(d => d.DonationId)
            .IsUnique();
        // BihzDonatie

        // BihzActie
        modelBuilder
            .Entity<BihzActie>()
            .Property(p => p.DoelBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzActie>()
            .Property(p => p.TotaalBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzActie>()
            .HasIndex(a => a.ActionId)
            .IsUnique();
        // BihzActie

        // BihzProject
        modelBuilder
            .Entity<BihzProject>()
            .Property(p => p.DoelBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzProject>()
            .Property(p => p.TotaalBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<BihzProject>()
            .HasIndex(p => p.ProjectId)
            .IsUnique();
        // BihzProject
    }

}
