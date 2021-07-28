using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.ComponentModel.DataAnnotations.Schema;
using MailMerge;
using System.IO;

namespace bihz.kantoorportaal.Data
{
    public class MergeTemplate
    {
        private List<string> _mergeFields = null;

        public Document MergeDocument { get; set; } 
        
        [NotMapped]
        public List<string> MergeFields 
        { 
            get
            {
                if (_mergeFields == null && MergeDocument.Content != null && MergeDocument.ContentType == ContentTypeEnum.Word)
                {
                    _mergeFields = GetMergeFields(MergeDocument.Content);
                }
                return _mergeFields;
            }
        }

        // code makes use of and is inspired by the Mailmerger code
        private List<string> GetMergeFields(byte[] document)
        {
            var mergeFields = new List<string>();
            
            var stream = new MemoryStream(document);
            var xdoc = MailMerger.GetMainDocumentPartXml(stream);
            var simpleMergeFields = xdoc.SelectNodes("//w:fldSimple[contains(@w:instr,'MERGEFIELD ')]", OoXmlNamespace.Manager);
            foreach (XmlNode node in simpleMergeFields)
            {
                var fieldName = node.Attributes[OoXPath.winstr]
                                    .Value
                                    .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                                    .Skip(1)
                                    .FirstOrDefault();
                mergeFields.Add(fieldName);
            }

            return mergeFields;
        }

    }
}
