using System;
using System.Collections.Generic;
using bihz.kantoorportaal.Data;

namespace bihz.kantoorportaal.Services
{
    public interface IMergeService
    {
        List<MergeTemplate> GetMergeTemplates();
        MergeTemplate GetMergeTemplateById(int id);
        MergeTemplate GetMergeTemplateByName(string name);
        void SaveMergeTemplate(MergeTemplate mergeTemplate);
        void DeleteMergeTemplate(int id);
        (bool, AggregateException) Merge(Dictionary<string, string> mergeFields);    
    }
}
