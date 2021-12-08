using System;
using System.IO;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIORenderer;
using Syncfusion.OfficeChart;
using Syncfusion.Pdf;

namespace BerghAdmin.Services
{
    public class PdfConverter : IPdfConverter
    {
        // public Stream ConvertWordToPdf(Stream inputStream)
        // {
        //     var doc = new Document();
        //     doc.LoadFromStream(inputStream, FileFormat.Docx, XHTMLValidationType.None);

        //     var pdfStream = new MemoryStream();
        //     doc.SaveToStream(pdfStream, FileFormat.PDF);

        //     return pdfStream;
        // }
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
}
