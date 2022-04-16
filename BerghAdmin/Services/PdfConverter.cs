using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.OfficeChart;
using Syncfusion.Pdf;

namespace BerghAdmin.Services;

public class PdfConverter : IPdfConverter
{

    // TO be done: maybe better error handling and logging?
    public Stream ConvertWordToPdf(Stream inputStream)
    {
        // Loads file stream into Word document
        WordDocument wordDocument = new(inputStream, Syncfusion.DocIO.FormatType.Automatic);
            
        // Instantiation of DocIORenderer for Word to PDF conversion
        DocIORenderer render = new();

        // Sets Chart rendering Options.
        render.Settings.ChartRenderingOptions.ImageFormat =  ExportImageFormat.Jpeg;

        // Converts Word document into PDF document
        PdfDocument pdfDocument = render.ConvertToPDF(wordDocument);

        // Releases all resources used by the Word document and DocIO Renderer objects
        render.Dispose();
        wordDocument.Dispose();

        // Saves the PDF file
        MemoryStream outputStream = new();
        pdfDocument.Save(outputStream);
        pdfDocument.Close();

        return outputStream;        
    }
}
