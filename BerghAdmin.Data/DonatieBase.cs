using BerghAdmin.Data.Kentaa;

namespace BerghAdmin.Data;

public class DonatieBase
{
    public DonatieBase()
    {
    }

    public int Id { get; set; } = 0;
    public decimal Bedrag { get; set; }
    public Donateur? Donateur { get; set; }
    public Factuur? Factuur { get; set; }
    public BihzDonatie? KentaaDonatie{ get; set; }
}
