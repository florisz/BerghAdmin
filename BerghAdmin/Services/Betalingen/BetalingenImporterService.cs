using CsvHelper;
using System.Globalization;

namespace BerghAdmin.Services.Betalingen;

public class BetalingenImporterService : IBetalingenImporterService
{
    private IBetalingenService _betalingenService;

    public BetalingenImporterService(IBetalingenService betalingenService)
    {
        _betalingenService = betalingenService;
    }

    public List<Betaling> ImportData(Stream csvData)
    {
        var betalingen = new List<Betaling>();

        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using StreamReader reader = new(csvData);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<RaboBetalingSpecificatie>().ToList<RaboBetalingSpecificatie>();

            foreach (var record in records)
            {
                betalingen.Add(ConvertToBetaling(record));
            }
        }
        catch (Exception)
        {
            // Let the user know what went wrong.
        }

        return betalingen;
    }

    private static Betaling ConvertToBetaling(RaboBetalingSpecificatie betalingSpecificatie)
        => new()
        {
            Id = 0,
            Bedrag = ConvertToDecimalType(betalingSpecificatie.Bedrag),
            BetalingType = BetalingTypeEnum.Bank,
            DatumTijd = ConvertToDateType(betalingSpecificatie.Datum),
            Munt = betalingSpecificatie.Munt,
            Volgnummer = betalingSpecificatie.Volgnr,
            Tegenrekening = betalingSpecificatie.TegenrekeningIBANBBAN,
            NaamTegenpartij = betalingSpecificatie.NaamTegenpartij,
            NaamUiteindelijkePartij = betalingSpecificatie.NaamUiteindelijkePartij,
            NaamInitierendePartij = betalingSpecificatie.NaamInitierendePartij,
            BICTegenpartij = betalingSpecificatie.BICTegenpartij,
            Code = betalingSpecificatie.Code,
            BatchID = betalingSpecificatie.BatchID,
            TransactieReferentie = betalingSpecificatie.Transactiereferentie,
            MachtigingsKenmerk = betalingSpecificatie.Machtigingskenmerk,
            IncassantID = betalingSpecificatie.IncassantID,
            BetalingsKenmerk = betalingSpecificatie.Betalingskenmerk,
            Omschrijving1 = betalingSpecificatie.Omschrijving1,
            Omschrijving2 = betalingSpecificatie.Omschrijving2,
            Omschrijving3 = betalingSpecificatie.Omschrijving3,
            RedenRetour = betalingSpecificatie.RedenRetour,
            OorspronkelijkBedrag = betalingSpecificatie.OorsprBedrag,
            OorspronkelijkMunt = betalingSpecificatie.OorsprMunt,
            Koers = betalingSpecificatie.Koers
        };

    private static DateTime? ConvertToDateType(string dateString)
    {
        if (DateTime.TryParseExact(dateString, "yyyy-MM-dd", null, DateTimeStyles.None, out var date))
            return date;

        return null;
    }

    private static Decimal ConvertToDecimalType(string decimalString)
    {
        var formatProvider = CultureInfo.CreateSpecificCulture("nl-NL").NumberFormat;
        if (Decimal.TryParse(decimalString,
                             NumberStyles.Any,
                             formatProvider,
                             out var amount))
            return amount;

        // TO BE DONE
        return (decimal)0;
    }
}
