using BerghAdmin.Data;
using BerghAdmin.ApplicationServices.KentaaInterface;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BerghAdmin.ApplicationServices.KentaaInterface.KentaaModel;

namespace BerghAdmin.DbContexts;

public class ApplicationDbContext : IdentityUserContext<Data.User, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {

    }

    public DbSet<Persoon>? Personen { get; set; }
    public DbSet<ApplicationServices.KentaaInterface.KentaaModel.User>?  Gebruikers { get; set; }
    public DbSet<Rol>? Rollen { get; set; }
    public DbSet<Document>? Documenten { get; set; }
    
    public DbSet<Factuur>? Facturen { get; set; }
    public DbSet<Betaling>? Betalingen { get; set; }
    public DbSet<VerzondenMail>? VerzondenMails { get; set; }
    public DbSet<Organisatie>? Organisaties { get; set; }
    public DbSet<Donatie>? Donaties { get; set; }
    public DbSet<Evenement>? Evenementen{ get; set; }
    public DbSet<KentaaDonatie>? KentaaDonaties{ get; set; }


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
            .Entity<KentaaDonatie>()
            .ToTable("KentaaDonaties");
        modelBuilder
            .Entity<Donatie>()
            .Property(p => p.Bedrag).HasPrecision(18, 2);
 
        modelBuilder
            .Entity<KentaaDonatie>()
            .Property(p => p.DonatieBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<KentaaDonatie>()
            .Property(p => p.TransactionKosten).HasPrecision(18, 2);
        modelBuilder
            .Entity<KentaaDonatie>()
            .Property(p => p.RegistratieFeeBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<KentaaDonatie>()
            .Property(p => p.TotaalBedrag).HasPrecision(18, 2);
        modelBuilder
            .Entity<KentaaDonatie>()
            .Property(p => p.NettoBedrag).HasPrecision(18, 2);

        modelBuilder
            .Entity<ApplicationServices.KentaaInterface.KentaaModel.Action>()
            .Property(p => p.TargetAmount).HasPrecision(18, 2);
        modelBuilder
            .Entity<ApplicationServices.KentaaInterface.KentaaModel.Action>()
            .Property(p => p.TotalAmount).HasPrecision(18, 2);

        modelBuilder
            .Entity<Donation>()
            .Property(p => p.Amount).HasPrecision(18, 2);
        modelBuilder
            .Entity<Donation>()
            .Property(p => p.TransactionCost).HasPrecision(18, 2);
        modelBuilder
            .Entity<Donation>()
            .Property(p => p.RegistrationFeeAmount).HasPrecision(18, 2);
        modelBuilder
            .Entity<Donation>()
            .Property(p => p.TotalAmount).HasPrecision(18, 2);
        modelBuilder
            .Entity<Donation>()
            .Property(p => p.ReceivableAmount).HasPrecision(18, 2);

    }

}
