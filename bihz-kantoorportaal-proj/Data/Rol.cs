using System.Collections.Generic;

namespace bihz.kantoorportaal.Data
{
    public class Rol
    {
        public int Id { get; set; }
        public string Beschrijving { get; set; }
        public HashSet<Persoon> Personen { get; set; }
    }
}
