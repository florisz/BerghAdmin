using System.Collections.Generic;
using System.IO;
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
        Stream Merge(MergeTemplate template, Dictionary<string, string> mergeFields);    
    }
}
