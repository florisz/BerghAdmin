using System;

namespace BerghAdmin.Data
{
    public class VerzondenMail
    {
        public int Id { get; set; }
        public string? Onderwerp { get; set; }
        public ICollection<Persoon>? Geadresseerden { get; set; }
        public ICollection<Persoon>? ccGeadresseerden { get; set; }
        public ICollection<Persoon>? bccGeadresseerden { get; set; }
        public DateTime? VerzendDatum { get; set; }
        public Document? Inhoud { get; set; }
    }
}
