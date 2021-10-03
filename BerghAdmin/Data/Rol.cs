using System.Collections.Generic;

namespace BerghAdmin.Data
{
    public class Rol
    {
        public int Id { get; set; }
        public string Beschrijving { get; set; }
        public string MeervoudBeschrijving { get; set; }
        public HashSet<Persoon> Personen { get; set; }
    }
}
