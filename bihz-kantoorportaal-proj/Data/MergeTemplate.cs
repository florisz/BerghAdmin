using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using Syncfusion.DocIO.DLS;
using Syncfusion.DocIO;

namespace bihz.kantoorportaal.Data
{
    public class MergeTemplate
    {
        private List<string> _mergeFields = null;

        public Document MergeDocument { get; set; } 
        
        private WordDocument _wordDocument;

        [NotMapped]
        public List<string> MergeFields 
        { 
            get
            {
                if (_mergeFields == null && MergeDocument.Content != null && MergeDocument.ContentType == ContentTypeEnum.Word)
                {
                    _mergeFields = GetMergeFields(new MemoryStream(MergeDocument.Content));
                }
                return _mergeFields;
            }
        }

        private List<string> GetMergeFields(Stream documentStream)
        {
            _wordDocument = new WordDocument(documentStream, FormatType.Docx);

            string[] fieldNames = _wordDocument.MailMerge.GetMergeFieldNames();

            return fieldNames.ToList<string>();
        }

    }
}
