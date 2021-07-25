using System;
using System.Collections.Generic;

namespace bihz.kantoorportaal.Data
{
    public enum ContentTypeEnum
    {
        Word,
        Excel,
        Html,
        Image,
        Text,
        Pdf
    }
    
    public class Document
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ContentTypeEnum ContentType { get; set; }
        public byte[] Content { get; set; }
        public bool IsMergeTemplate { get; set; }
    }
}
