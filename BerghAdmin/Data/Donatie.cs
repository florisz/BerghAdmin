using System;

namespace BerghAdmin.Data;

public class Donatie
{
#pragma warning disable IDE0060 // Remove unused parameter
    public Donatie(int id, DateTime datum, decimal bedrag)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        Bedrag = bedrag;
        Datum = datum;
    }

    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public decimal Bedrag { get; set; }
    public Donateur? Donateur { get; set; }
    public KentaaDonatie? KentaaDonatie { get; set; }
    public Factuur? Factuur { get; set; }
}
