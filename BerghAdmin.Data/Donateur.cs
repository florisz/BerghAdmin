using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data
{
    public abstract class Donateur
    {
        public int Id { get; set; }
        public bool IsVerwijderd { get; set; } = false;
        public string? Adres { get; set; }
        public string? Postcode  { get; set; }
        public string? Plaats  { get; set; }
        public string? Land  { get; set; }
        public IEnumerable<DonatieBase> Donaties { get; set; }
        [NotMapped]
        public decimal? GetDonatieBedrag
        {
            get
            { 
                var bedrag = Donaties?.Sum(d => d.Bedrag);
                
                return bedrag ?? 0;
            }
        }
    }
}
