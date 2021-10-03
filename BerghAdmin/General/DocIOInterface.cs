using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;

namespace BerghAdmin.General
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
