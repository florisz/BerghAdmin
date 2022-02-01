
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace BerghAdmin.ApplicationServices.DocIO
{
    public static class DocIOInterface
    {
        public static List<string> GetMergeFields(Stream documentStream)
        {
            var wordDocument = new WordDocument(documentStream, FormatType.Docx);

            string[] fieldNames = wordDocument.MailMerge.GetMergeFieldNames();

            return fieldNames.ToList<string>();
        }
    }
}
