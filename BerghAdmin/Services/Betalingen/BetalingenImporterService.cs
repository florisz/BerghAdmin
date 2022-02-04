using CsvHelper;
using System.Globalization;

namespace BerghAdmin.Services.Betalingen;

public class BetalingenImporterService : IBetalingenImporterService
{
    public void ImportData(Stream csvData)
    {
        try
        {
            // Create an instance of StreamReader to read from a file.
            // The using statement also closes the StreamReader.
            using StreamReader reader = new(csvData);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            var records = csv.GetRecords<RaboBetalingSpecificatie>().ToList<RaboBetalingSpecificatie>();
            foreach (var record in records)
            {
                var betaling = new Betaling()
                {
                    Bedrag = ConvertToDecimalType(record.Bedrag),
                    BetalingType = BetalingTypeEnum.Bank,
                    DatumTijd = ConvertToDateType(record.Datum),
                    Id = 0
                };


                //_betalingService.AddBetaling(betaling);
            }
        }
        catch (Exception)
        {
            // Let the user know what went wrong.
        }
    }

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
        return (decimal) 0;
    }
}
