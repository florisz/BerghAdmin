using BerghAdmin.DbContexts;

namespace BerghAdmin.Services;

public class DocumentService : IDocumentService
{
    private readonly ILogger<DocumentService> _logger;
    private readonly ApplicationDbContext _dbContext;

    public DocumentService(ApplicationDbContext context, ILogger<DocumentService> logger)
    {
        _dbContext = context;
        _logger = logger;
    }
        
    public void DeleteDocument(int id)
    {
        throw new NotImplementedException();
    }

    public Document? GetDocumentById(int id)
    {
        _logger.LogDebug("Get document {documentId}", id);

        var doc = _dbContext
                    .Documenten?
                    .Find(id);

        _logger.LogInformation("Get document with id {documentId} was {result}", id, doc == null? "NOT Ok" : "Ok");

        return doc;
    }

    public Document? GetDocumentByName(string name)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Document> GetDocuments()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<Document>? GetMergeTemplates()
    {
        var templates = _dbContext
                    .Documenten?
                    .Where(d => d.TemplateType != TemplateTypeEnum.None)
                    .ToList<Document>();
                        
        return templates;
    }

    public void SaveDocument(Document document)
    {
        _logger.LogDebug("Save document merge {documentName}", document.Name);

        if (document.Id == 0) 
        {
            _dbContext.Documenten?.Add(document);
            _logger.LogInformation("Document {documentName} added", document.Name);
        }
        else
        { 
            _dbContext.Documenten?.Update(document);
            _logger.LogInformation("Document {documentName} updated", document.Name);
        }
        _dbContext.SaveChanges();
    }
}
