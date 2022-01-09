using System;

namespace BerghAdmin.Data
{
    public abstract class Evenement
    {
        public int Id { get; set; }
        public string? Naam { get; set; }
        public HashSet<Persoon> Deelnemers { get; set;} = new();
    }
}
