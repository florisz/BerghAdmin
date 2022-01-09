using System;

namespace BerghAdmin.Data
{
    public class Donatie
    {
        public int Id { get; set; }
        public DateTime? Datum { get; set; }
        public float? Bedrag { get; set; }
        public Donateur? Donateur { get; set; }
    }
}
