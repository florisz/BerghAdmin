namespace BerghAdmin.Services.Betalingen;

using CsvHelper.Configuration.Attributes;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class RaboBetalingSpecificatie
{
	[Name("IBAN/BBAN")]
	public string IBANBBAN { get; set; }

	[Name("Munt")]
	public string Munt { get; set; }

	[Name("BIC")]
	public string BIC { get; set; }

	[Name("Volgnr")]
	public string Volgnr { get; set; }

	[Name("Datum")]
	public string Datum { get; set; }

	[Name("Rentedatum")]
	public string Rentedatum { get; set; }

	[Name("Bedrag")]
	public string Bedrag { get; set; }

	[Name("Saldo na trn")]
	public string SaldoTarn { get; set; }

	[Name("Tegenrekening IBAN/BBAN")]
	public string TegenrekeningIBANBBAN { get; set; }

	[Name("Naam tegenpartij")]
	public string NaamTegenpartij { get; set; }

	[Name("Naam uiteindelijke partij")]
	public string NaamUiteindelijkePartij { get; set; }

	[Name("Naam initiërende partij")]
	public string NaamInitierendePartij { get; set; }

	[Name("BIC tegenpartij")]
	public string BICTegenpartij { get; set; }

	[Name("Code")]
	public string Code { get; set; }

	[Name("Batch ID")]
	public string BatchID { get; set; }

	[Name("Transactiereferentie")]
	public string Transactiereferentie { get; set; }

	[Name("Machtigingskenmerk")]
	public string Machtigingskenmerk { get; set; }
	
	[Name("Incassant ID")]
	public string IncassantID { get; set; }

	[Name("Betalingskenmerk")]
	public string Betalingskenmerk { get; set; }

	[Name("Omschrijving-1")]
	public string Omschrijving1 { get; set; }

	[Name("Omschrijving-2")]
	public string Omschrijving2 { get; set; }

	[Name("Omschrijving-3")]
	public string Omschrijving3 { get; set; }

	[Name("Reden retour")]
	public string RedenRetour   { get; set; }

	[Name("Oorspr bedrag")]
	public string OorsprBedrag { get; set; }

	[Name("Oorspr munt")]
	public string OorsprMunt { get; set; }

	[Name("Koers")]
	public string Koers { get; set; }
}