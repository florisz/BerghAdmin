using System;
using System.Collections.Generic;
using bihz.kantoorportaal.Data;
using bihz.kantoorportaal.DbContexts;

namespace bihz.kantoorportaal.Services
{
    public class MergeService : IMergeService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IDocumentService _documentService;

        public MergeService(ApplicationDbContext context, IDocumentService documentService)
        {
            _dbContext = context;
            _documentService = documentService;
        }

        public void DeleteMergeTemplate(int id)
        {
            throw new NotImplementedException();
        }

        public MergeTemplate GetMergeTemplateById(int id)
        {
            throw new NotImplementedException();
        }

        public MergeTemplate GetMergeTemplateByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<MergeTemplate> GetMergeTemplates()
        {
            throw new NotImplementedException();
        }

        public (bool, AggregateException) Merge(Dictionary<string, string> mergeFields)
        {
            throw new NotImplementedException();
        }

        public void SaveMergeTemplate(MergeTemplate mergeTemplate)
        {
            throw new NotImplementedException();
        }
    }
}
