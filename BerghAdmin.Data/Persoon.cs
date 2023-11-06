using BerghAdmin.Data.Kentaa;
using System.ComponentModel.DataAnnotations.Schema;

namespace BerghAdmin.Data;

public class Persoon : Donateur
{
    public Persoon() : base()
    {
        Geslacht = GeslachtEnum.Onbekend;
        Rollen = new HashSet<Rol>();
        Fietstochten = new List<Fietstocht>();
        Golfdagen = new List<Golfdag>();
    }

    public GeslachtEnum Geslacht { get; set; }
    public string? Voorletters { get; set; }
    public string? Voornaam { get; set; }
    public string? Achternaam { get; set; }
    public string? Tussenvoegsel { get; set; }
    public DateTime? GeboorteDatum { get; set; }
    public string? EmailAdresExtra { get; set; }
    public string? KledingMaten { get; set; }
    public string? Nummer { get; set; }
    // twee velden alleen gebruiken voor golfers
    public string? Handicap{ get; set; }
    public bool Buggy { get; set; } = false;
    //
    public HashSet<Rol> Rollen { get; set; } = new HashSet<Rol>();
    public ICollection<VerzondenMail> Geadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<VerzondenMail> ccGeadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<VerzondenMail> bccGeadresseerden { get; set; } = new List<VerzondenMail>();
    public ICollection<Fietstocht> Fietstochten { get; set; } = new List<Fietstocht>();
    public ICollection<Golfdag> Golfdagen { get; set; } = new List<Golfdag>();
    public BihzActie? BihzActie { get; set; }
    public BihzUser? BihzUser { get; set; }
    public BihzProject? Project { get; set; }
    public bool IsReserve { get; set; } = false;

    [NotMapped]
    public int[] SelectedFietsTochten => Fietstochten.Select(f => f.Id).ToArray<int>();
    [NotMapped]
    public int[] SelectedRollen => Rollen.Select(r => r.Id).ToArray<int>();

    [NotMapped]
    public string GetRollenAsString
        => string.Join(", ", Rollen.Select(r => r.Beschrijving));

    [NotMapped]
    public string VolledigeNaam
        => string.Join(" ", new string?[] {
                                Voornaam,
                                string.IsNullOrEmpty(Voorletters)? "" : $"({Voorletters})",
                                Tussenvoegsel,
                                Achternaam }
        );

    [NotMapped]
    public string VolledigeNaamMetRollen
         => $"{VolledigeNaam} ( {GetRollenAsString} )";

    public static Persoon Empty
        => new Persoon()
        {
            Achternaam = "Onbekend",
            Voornaam = "Onbekend",
        };
    [NotMapped]
    public IEnumerable<Fietstocht> GetFietstochten
    {
        get
        {
            return this.Fietstochten.OfType<Fietstocht>();
        }
    }

}