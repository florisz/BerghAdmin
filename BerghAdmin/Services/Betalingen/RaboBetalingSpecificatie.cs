﻿namespace BerghAdmin.Services.Betalingen;
using CsvHelper.Configuration.Attributes;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public class RaboBetalingSpecificatie
{
	[Name("IBAN/BBAN")]
	public string IBANBBAN { get; set; }

	public string Munt { get; set; }

	public string BIC { get; set; }

	public string Volgnr { get; set; }

	public string Datum { get; set; }

	public string Rentedatum { get; set; }

	public string Bedrag { get; set; }

	public string SaldoTarn { get; set; }

	public string TegenrekeningIBANBBAN { get; set; }

	public string NaamTegenpartij { get; set; }

	public string NaamUiteindelijkePartij { get; set; }

	public string NaamInitierendePartij { get; set; }

	public string BICTegenpartij { get; set; }

	public string Code { get; set; }

	public string BatchID { get; set; }

	public string Transactiereferentie { get; set; }

	public string Machtigingskenmerk { get; set; }
	
	[Name("Incassant ID")]
	public string IncassantID { get; set; }

	public string Betalingskenmerk { get; set; }

	public string Omschrijving1 { get; set; }

	public string Omschrijving2 { get; set; }

	public string Omschrijving3 { get; set; }

	public string RedenRetour   { get; set; }

	public string OorsprBedrag { get; set; }

	public string OorsprMunt { get; set; }

	public string Koers { get; set; }
}