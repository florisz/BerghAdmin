using System;
using System.Collections.Generic;
using BIHZ.AdminPortaal.Data;
using Microsoft.EntityFrameworkCore;

namespace BIHZ.AdminPortaal.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Persoon> Personen { get; set; }
        public DbSet<Rol> Rollen { get; set; }
        public DbSet<PersoonRol> PersoonRol { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<PersoonRol>()
                .HasKey(pr => new { pr.PersoonId, pr.RolId });
                
            modelBuilder
                .Entity<Persoon>()
                .HasMany(p => p.Rollen)
                .WithMany(r => r.Personen)
                .UsingEntity(j => j.ToTable("PersoonInRol"));
                
                // .UsingEntity<Dictionary<string, object>>(
                //     "PersoonRol",
                //     j => j
                //         .HasOne<Rol>()
                //         .WithMany()
                //         .HasForeignKey("RolId")
                //         .HasConstraintName("FK_PersoonRol_Rollen_RolId")
                //         .OnDelete(DeleteBehavior.Cascade),
                //     j => j
                //         .HasOne<Persoon>()
                //         .WithMany()
                //         .HasForeignKey("PersoonId")
                //         .HasConstraintName("FK_PersoonRol_Personen_PersoonId")
                //         .OnDelete(DeleteBehavior.ClientCascade)
                // );

            modelBuilder
                .Entity<Rol>()
                .HasData(
                    new Rol { Id = 1, Beschrijving = "Renner"},
                    new Rol { Id = 2, Beschrijving = "Begeleider"},
                    new Rol { Id = 3, Beschrijving = "Reserve"},
                    new Rol { Id = 4, Beschrijving = "Commissielid"},
                    new Rol { Id = 5, Beschrijving = "Vriend van"},
                    new Rol { Id = 6, Beschrijving = "Mailing abonnee"},
                    new Rol { Id = 7, Beschrijving = "Golfer"},
                    new Rol { Id = 8, Beschrijving = "Ambassadeur"}
                );

            // Testdata
            modelBuilder
                .Entity<Persoon>()
                .HasData(
                    new Persoon { 
                        Id = 1, 
                        Voorletters = "A. B.",
                        Voornaam = "Appie",
                        Achternaam = "Happie", 
                        Adres = "Straat 1", 
                        EmailAdres = "ahappie@mail.com", 
                        GeboorteDatum = new DateTime(1970, 1, 1), 
                        Geslacht = GeslachtEnum.Man, 
                        Land = "Nederland", 
                        Mobiel = "06-12345678", 
                        Plaats = "Amsterdam",
                        Postcode = "1234 AB",
                        Telefoon = "onbekend"
                    },
                    new Persoon { 
                        Id = 2, 
                        Voorletters = "B.",
                        Voornaam = "Bert",
                        Achternaam = "Bengel", 
                        Adres = "Straat 2", 
                        EmailAdres = "bbengel@mail.com", 
                        GeboorteDatum = new DateTime(1970, 1, 1), 
                        Geslacht = GeslachtEnum.Man, 
                        Land = "Nederland", 
                        Mobiel = "06-12345678", 
                        Plaats = "Rotterdam",
                        Postcode = "4321 AB",
                        Telefoon = "onbekend"
                    }
                );

        }

    }
}
