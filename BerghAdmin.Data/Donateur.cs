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
        public int? DebiteurNummer { get; set; }
        public bool IsVerwijderd { get; set; } = false;
        public string? SponsorNaam { get; set; }
        public string? Adres { get; set; }
        public string? Postcode  { get; set; }
        public string? Plaats  { get; set; }
        public string? Land  { get; set; }
        public string? Telefoon { get; set; }
        public string? Mobiel { get; set; }
        public string EmailAdres { get; set; }
        public string? EmailAdresExtra { get; set; }
        public decimal? BedragToegezegd { get; set; }
        public DateTime? DatumAangebracht { get; set; }
        public string? Opmerkingen {  get; set; }
        public IEnumerable<Donatie> Donaties { get; set; }

        [NotMapped]
        public decimal? GetDonatieBedrag
            => Donaties.Sum(d => d.Bedrag);
    }
}
