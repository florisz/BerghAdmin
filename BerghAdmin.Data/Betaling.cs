namespace BerghAdmin.Data;

public class Betaling
{
    public int Id { get; set; }
    public decimal Bedrag { get; set; }
    public DateTime? DatumTijd { get; set; }
    public BetalingTypeEnum BetalingType { get; set; }
}
