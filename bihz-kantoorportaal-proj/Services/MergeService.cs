using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using bihz.kantoorportaal.Data;
using bihz.kantoorportaal.DbContexts;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

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

        public Document GetMergeTemplateById(int id)
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

            return doc;
        }

        public Document GetMergeTemplateByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Document> GetMergeTemplates()
        {
            throw new NotImplementedException();
        }

        public Stream Merge(Document template, Dictionary<string, string> mergeFields)
        {
            var inputStream = new MemoryStream(template.Content);
            
            var fieldNames = mergeFields.Keys.ToArray<string>();
            var fieldValues = mergeFields.Values.ToArray<string>();

            var wordDocument = new WordDocument(inputStream, FormatType.Docx);
            wordDocument.MailMerge.Execute(fieldNames, fieldValues);

            var outputStream = new MemoryStream();
            wordDocument.Save(outputStream, FormatType.Docx);

            return outputStream;
        }

        public void SaveMergeTemplate(Document mergeTemplate)
        {
            throw new NotImplementedException();
        }
    }
}
