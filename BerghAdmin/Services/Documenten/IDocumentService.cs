namespace BerghAdmin.Services.Documenten
{
    public interface IDocumentService
    {
        IEnumerable<Document>? GetDocuments();
        IEnumerable<Document>? GetMergeTemplates();

        Document? GetDocumentById(int id);
        Document? GetDocumentByName(string name);

        Task SaveDocument(Document document);
        Task DeleteDocument(int id);
    }
}
