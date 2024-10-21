using ClosedXML.Excel;
using Microsoft.JSInterop;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;

namespace BerghAdmin.Services.Export;

public class ExcelService : IExcelService
{
    private readonly IPersoonService _persoonService;
    private ILogger<ExcelService> _logger;
    private Microsoft.JSInterop.IJSRuntime _jsRuntime;

    public ExcelService(IPersoonService persoonService, Microsoft.JSInterop.IJSRuntime jsRuntime, ILogger<ExcelService> logger)
    {
        _persoonService = persoonService;
        _jsRuntime = jsRuntime;
        _logger = logger;
        logger.LogDebug($"ExcelService created; threadid={Thread.CurrentThread.ManagedThreadId}");
    }

    public async Task<bool> ExportPersonenAsync(string fileName)
    {
        if (!IsValidFileName(fileName))
        { 
            this._logger.LogError($"Invalid fileName: {fileName}");
            return false;
        }
        
        var personen = await _persoonService.GetPersonen();
        personen = personen
                    .Where(p => p.IsVerwijderd == false)
                    .OrderBy(p => p.Achternaam).ToList();

        using (var workbook = new XLWorkbook())
        {
            var worksheet = workbook.Worksheets.Add("Personen");

            CreateHeader(worksheet);

            AddPersonenData(worksheet, personen);

            //workbook.SaveAs(fileName);

            using (MemoryStream stream = new MemoryStream())
            {
                //Save the created Excel document to MemoryStream
                workbook.SaveAs(stream);

                //Download the excel file.
                await _jsRuntime.SaveAs(fileName, stream.ToArray());
            }

        }

        return true;
    }

    private void CreateHeader(IXLWorksheet worksheet)
    {
        var col = 1;
        worksheet.Cell(1, col++).Value = "VolledigeNaam";
        worksheet.Cell(1, col++).Value = "Voornaam";
        worksheet.Cell(1, col++).Value = "Voorletters";
        worksheet.Cell(1, col++).Value = "Tussenvoegsel";
        worksheet.Cell(1, col++).Value = "Achternaam";
        worksheet.Cell(1, col++).Value = "EmailAdres";
        worksheet.Cell(1, col++).Value = "EmailAdresExtra";
        worksheet.Cell(1, col++).Value = "Adres";
        worksheet.Cell(1, col++).Value = "Postcode";
        worksheet.Cell(1, col++).Value = "Plaats";
        worksheet.Cell(1, col++).Value = "Land";
        worksheet.Cell(1, col++).Value = "Telefoon";
        worksheet.Cell(1, col++).Value = "Mobiel";
        worksheet.Cell(1, col++).Value = "Opmerkingen";
        worksheet.Cell(1, col++).Value = "Geslacht";
        worksheet.Cell(1, col++).Value = "GeboorteDatum";
        worksheet.Cell(1, col++).Value = "Rollen";
        worksheet.Cell(1, col++).Value = "Fietstochten";
        worksheet.Cell(1, col++).Value = "Golfdagen";
        worksheet.Cell(1, col++).Value = "IsReserve";
        worksheet.Cell(1, col++).Value = "KledingMaten";
        worksheet.Cell(1, col++).Value = "Nummer";
        worksheet.Cell(1, col++).Value = "Handicap";
        worksheet.Cell(1, col++).Value = "Buggy";
    }

    private void AddPersonenData(IXLWorksheet worksheet, List<Persoon> personen)
    {
        var row = 2;
        foreach (var persoon in personen)
        {
            var col = 1;
            worksheet.Cell(row, col++).Value = persoon.VolledigeNaam;
            worksheet.Cell(row, col++).Value = persoon.Voornaam;
            worksheet.Cell(row, col++).Value = persoon.Voorletters;
            worksheet.Cell(row, col++).Value = persoon.Tussenvoegsel;
            worksheet.Cell(row, col++).Value = persoon.Achternaam;
            worksheet.Cell(row, col++).Value = persoon.EmailAdres;
            worksheet.Cell(row, col++).Value = persoon.EmailAdresExtra;
            worksheet.Cell(row, col++).Value = persoon.Adres;
            worksheet.Cell(row, col++).Value = persoon.Postcode;
            worksheet.Cell(row, col++).Value = persoon.Plaats;
            worksheet.Cell(row, col++).Value = persoon.Land;
            worksheet.Cell(row, col++).Value = persoon.Telefoon;
            worksheet.Cell(row, col++).Value = persoon.Mobiel;
            worksheet.Cell(row, col++).Value = persoon.Opmerkingen;
            worksheet.Cell(row, col++).Value = persoon.Geslacht.ToString();
            worksheet.Cell(row, col++).Value = persoon.GeboorteDatum;
            worksheet.Cell(row, col++).Value = persoon.GetRollenAsString;
            worksheet.Cell(row, col++).Value = persoon.GetFietstochtenAsString;
            worksheet.Cell(row, col++).Value = persoon.GetGolfdagenAsString;
            worksheet.Cell(row, col++).Value = persoon.IsReserve;
            worksheet.Cell(row, col++).Value = persoon.KledingMaten;
            worksheet.Cell(row, col++).Value = persoon.Nummer;
            worksheet.Cell(row, col++).Value = persoon.Handicap;
            worksheet.Cell(row, col++).Value = persoon.Buggy;
            row++;
        }
    }

    public static bool IsValidFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
        {
            return false;
        }

        char[] invalidChars = Path.GetInvalidFileNameChars();

        foreach (char c in invalidChars)
        {
            if (fileName.Contains(c))
            {
                return false;
            }
        }

        return true;
    }
}