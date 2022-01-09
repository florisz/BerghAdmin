using System;

namespace BerghAdmin.Data
{
    public abstract class Donateur
    {
        public Donateur()
        {
            IsVerwijderd = false;
        }
        
        public int Id { get; set; }
        public bool IsVerwijderd { get; set; }
        public string? Adres { get; set; }
        public string? Postcode  { get; set; }
        public string? Plaats  { get; set; }
        public string? Land  { get; set; }
    }
}
