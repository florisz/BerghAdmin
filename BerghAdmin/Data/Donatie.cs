namespace BerghAdmin.Data;

public class Donatie
{
    public Donatie()
    {
    }

    public int Id { get; set; } = 0;
    public decimal Bedrag { get; set; }
    public Donateur? Donateur { get; set; }
    public Factuur? Factuur { get; set; }
    public KentaaDonation? KentaaDonatie{ get; set; }
}
