
namespace BerghAdmin.Data;

public enum GeslachtEnum
{
    Onbekend,
    Man,
    Vrouw
}

public class Geslacht
{
    public GeslachtEnum GeslachtValue { get; set; }
    public string GeslachtText { get; set; } = "";
}

public static class GeslachtService
{
    public static Geslacht[] GetGeslachtValues()
    => new Geslacht[]
        {
            new Geslacht() { GeslachtValue = GeslachtEnum.Man, GeslachtText = GeslachtEnum.Man.ToString() },
            new Geslacht() { GeslachtValue = GeslachtEnum.Vrouw, GeslachtText = GeslachtEnum.Vrouw.ToString() },
            new Geslacht() { GeslachtValue = GeslachtEnum.Onbekend, GeslachtText = GeslachtEnum.Onbekend.ToString() }
        };
}

