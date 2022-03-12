using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data
{
    public abstract class Donateur
    {
        public Donateur()
        {
            Donaties = new List<Donatie>();
        }

        public int Id { get; set; }
        public bool IsVerwijderd { get; set; } = false;
        public string? Adres { get; set; }
        public string? Postcode  { get; set; }
        public string? Plaats  { get; set; }
        public string? Land  { get; set; }
        public IEnumerable<Donatie> Donaties { get; set; }

        [NotMapped]
        public decimal? GetDonatieBedrag
            => Donaties.Sum(d => d.Bedrag);
    }
}
