using System;
using System.Collections.Generic;
using System.Linq;

using BerghAdmin.Data;
using BerghAdmin.DbContexts;

namespace BerghAdmin.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ApplicationDbContext _dbContext;

        public DocumentService(ApplicationDbContext context)
        {
            _dbContext = context;
        }
        
        public void DeleteDocument(int id)
        {
            throw new NotImplementedException();
        }

        public Document GetDocumentById(int id)
        {
            var doc = _dbContext
                        .Documenten
                        .Find(id);
                        
            return doc;
        }

        public Document GetDocumentByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Document> GetDocuments()
        {
            throw new NotImplementedException();
        }

        public List<Document> GetMergeTemplates()
        {
            var templates = _dbContext
                        .Documenten
                        .Where(d => d.TemplateType != TemplateTypeEnum.None)
                        .ToList<Document>();
                        
            return templates;
        }

        public void SaveDocument(Document document)
        {
            if (document.Id == 0) 
            {
                _dbContext.Documenten.Add(document);
            }
            else
            { 
                _dbContext.Documenten.Update(document);
            }
            _dbContext.SaveChanges();
        }
    }
}
