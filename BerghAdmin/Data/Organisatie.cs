using System;

namespace BerghAdmin.Data
{
    public class Organisatie : Donateur
    {
        public string? Naam { get; set; }
        public Persoon? ContactPersoon { get; set; } 
    }
}
