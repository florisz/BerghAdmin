using System;
using System.Collections.Generic;
using bihz.kantoorportaal.Data;
using bihz.kantoorportaal.DbContexts;

namespace bihz.kantoorportaal.Services
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

        public Document GetMergeTemplates()
        {
            throw new NotImplementedException();
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
