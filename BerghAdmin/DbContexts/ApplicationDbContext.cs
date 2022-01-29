using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.DbContexts;

public class ApplicationDbContext : IdentityUserContext<Data.User, int>
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
    public DbSet<DonatieBase>? Donaties { get; set; }
    public DbSet<Evenement>? Evenementen{ get; set; }
    public DbSet<BihzActie>? BihzActies { get; set; }
    public DbSet<BihzDonatie>? BihzDonaties { get; set; }
    public DbSet<BihzProject>? BihzProjects { get; set; }
    public DbSet<BihzUser>? BihzUsers { get; set; }


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
            .Entity<FietsTocht>()
            .ToTable("FietsTochten");

        modelBuilder
            .Entity<GolfDag>()
            .ToTable("GolfDagen");

        modelBuilder
            .Entity<BihzDonatie>()
            .ToTable("BihzDonaties");
        modelBuilder
            .Entity<DonatieBase>()
            .Property(p => p.Bedrag).HasPrecision(18, 2);
 
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


    }

}
