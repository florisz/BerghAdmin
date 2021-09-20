using System.Collections.Generic;
using System.IO;
using bihz.kantoorportaal.Data;

namespace bihz.kantoorportaal.Services
{
    public interface IMergeService
    {
        List<Document> GetMergeTemplates();
        Document GetMergeTemplateById(int id);
        Document GetMergeTemplateByName(string name);
        void SaveMergeTemplate(Document mergeTemplate);
        void DeleteMergeTemplate(int id);
        Stream Merge(Document template, Dictionary<string, string> mergeFields);    
    }
}
