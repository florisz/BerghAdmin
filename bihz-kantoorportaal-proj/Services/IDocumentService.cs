using System;
using System.Collections.Generic;
using bihz.kantoorportaal.Data;

namespace bihz.kantoorportaal.Services
{
    public interface IDocumentService
    {
        List<Document> GetDocuments();
        Document GetDocumentById(int id);
        Document GetDocumentByName(string name);
        void SaveDocument(Document document);
        void DeleteDocument(int id);    
    }
}
