using System;

namespace BerghAdmin.Data;

#pragma warning disable IDE0060 // Remove unused parameter
public class Donatie
{
    public Donatie(DateTime datum, decimal bedrag)
    {
        Id = 0;
        Bedrag = bedrag;
        Datum = datum;
    }

    public int Id { get; set; }
    public DateTime Datum { get; set; }
    public decimal Bedrag { get; set; }
    public Donateur? Donateur { get; set; }
    public int? KentaaActionId { get; set; }
    public KentaaDonatie? KentaaDonatie { get; set; }
    public Factuur? Factuur { get; set; }
}
#pragma warning restore IDE0060 // Remove unused parameter
