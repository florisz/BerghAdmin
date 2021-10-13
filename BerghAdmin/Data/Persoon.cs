using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace BerghAdmin.Data
{
    public enum GeslachtEnum
    {
        Onbekend,
        Man,
        Vrouw
    }

    public class Persoon
    {
        public int Id { get; set; }
        public GeslachtEnum Geslacht { get; set; }
        public string Voorletters { get; set; }
        public string Voornaam { get; set; }
        public string Achternaam { get; set; }
        public string Tussenvoegsel { get; set; }
        public DateTime? GeboorteDatum { get; set; }
        public string Adres { get; set; }
        public string Postcode  { get; set; }
        public string Plaats  { get; set; }
        public string Land  { get; set; }
        public string Telefoon  { get; set; }
        public string Mobiel  { get; set; }
        public string EmailAdres  { get; set; } 
        public string EmailAdresExtra  { get; set; } 
        public bool IsVerwijderd { get; set; }
        public HashSet<Rol> Rollen { get; set; }
       
        [NotMapped]
        public string GetRollenAsString
        {
            get { return Rollen == null? null : string.Join(", ", Rollen.Select(r => r.Beschrijving)); }
        }

        [NotMapped]
        public string VolledigeNaam
        {
            get { return string.Join(" ", new string[] { Voornaam, string.IsNullOrEmpty(Voorletters)? "" : $"({Voorletters})", Tussenvoegsel, Achternaam } ); }
        }
    }
}

