using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
                    _mergeFields = GetMergeFields();
                }
                return _mergeFields;
            }
        }

        private static List<string> GetMergeFields()
        {
            var mergeFields = new List<string>();
            
            // to do scan word document and return list with all mergefields

            return mergeFields;
        }
    }
}
