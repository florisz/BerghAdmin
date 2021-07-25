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
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }
    }
}
