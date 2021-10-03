using System;
using System.Collections.Generic;
using BerghAdmin.Data;

namespace BerghAdmin.Services
{
    public interface IDocumentService
    {
        List<Document> GetDocuments();
        List<Document> GetMergeTemplates();
        
        Document GetDocumentById(int id);
        Document GetDocumentByName(string name);

        void SaveDocument(Document document);
        void DeleteDocument(int id);    
    }
}
