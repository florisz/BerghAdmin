using System;
using System.Collections.Generic;
using bihz.kantoorportaal.Data;
using Microsoft.EntityFrameworkCore;

namespace bihz.kantoorportaal.DbContexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Persoon> Personen { get; set; }
        public DbSet<Rol> Rollen { get; set; }
        public DbSet<Document> Documenten { get; set; }
    }
}
