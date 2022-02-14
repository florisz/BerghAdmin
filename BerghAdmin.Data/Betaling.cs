namespace BerghAdmin.Data;

public class Betaling
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public int Id { get; set; }
    public decimal Bedrag { get; set; }
    public DateTime? DatumTijd { get; set; }
    public BetalingTypeEnum BetalingType { get; set; }
    public string Munt { get; set; }
    public string Volgnummer { get; set; }
    public string Tegenrekening { get; set; }
	public string NaamTegenpartij { get; set; }
	public string NaamUiteindelijkePartij { get; set; }
	public string NaamInitierendePartij { get; set; }
	public string BICTegenpartij { get; set; }
	public string Code { get; set; }
	public string BatchID { get; set; }
	public string TransactieReferentie { get; set; }
	public string MachtigingsKenmerk { get; set; }
	public string IncassantID { get; set; }
	public string BetalingsKenmerk { get; set; }
	public string Omschrijving1 { get; set; }
	public string Omschrijving2 { get; set; }
	public string Omschrijving3 { get; set; }
	public string RedenRetour { get; set; }
	public string OorspronkelijkBedrag { get; set; }
	public string OorspronkelijkMunt { get; set; }
	public string Koers { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
