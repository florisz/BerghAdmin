using System;

namespace BerghAdmin.Data
{
    public class Factuur
    {
        public Factuur()
        {
            Nummer = GenerateFactuurNummer();
        }
        public int Id { get; set; }
        public string Nummer { get; set; }
        public string? Omschrijving { get; set; }
        public float? Bedrag { get; set; }
        public DateTime? Datum { get; set; }
        public bool IsVerzonden { get; set; }
        public FactuurTypeEnum FactuurType { get; set; }
        public Document? EmailTekst { get; set; }
        public Document? FactuurTekst { get; set; }
        // TO DO: work out logic
        public bool IsBetaald()
        {
           return true; 
        }

        private string GenerateFactuurNummer()
        {
            return "2022-1";
        }
    }
}
