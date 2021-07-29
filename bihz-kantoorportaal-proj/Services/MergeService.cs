using System;
using System.Collections.Generic;
using System.IO;
using bihz.kantoorportaal.Data;
using bihz.kantoorportaal.DbContexts;
using MailMerge;

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
            var doc = _documentService.GetDocumentById(id);
            if (doc == null)
            {
                return null;
            }
            
            if (! doc.IsMergeTemplate)
            {
                throw new ArgumentException($"document with name {doc.Name} is not a mergetemplate");
            }

            return new MergeTemplate { MergeDocument = doc };
        }

        public MergeTemplate GetMergeTemplateByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<MergeTemplate> GetMergeTemplates()
        {
            throw new NotImplementedException();
        }

        public Stream Merge(MergeTemplate template, Dictionary<string, string> mergeFields)
        {
            var inputStream = new MemoryStream(template.MergeDocument.Content);
            
            var (outputStream, errors) = new MailMerger().Merge(inputStream, mergeFields); 
            if (errors.InnerExceptions.Count > 0)
            {
                throw errors;
            }

            return outputStream;
        }

        public void SaveMergeTemplate(MergeTemplate mergeTemplate)
        {
            throw new NotImplementedException();
        }
    }
}
