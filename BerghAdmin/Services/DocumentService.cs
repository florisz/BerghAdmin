using BerghAdmin.DbContexts;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services;

public class DocumentService : IDocumentService
{
    private readonly ILogger<DocumentService> _logger;
    private readonly ApplicationDbContext _dbContext;

    public DocumentService(ApplicationDbContext dbContext, ILogger<DocumentService> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
        logger.LogDebug($"DocumentService created; threadid={Thread.CurrentThread.ManagedThreadId}, dbcontext={_dbContext.ContextId}");
    }

    public Task DeleteDocument(int id)
    {
        throw new NotImplementedException();
    }

    public Document? GetDocumentById(int id)
    {
        _logger.LogDebug($"Get document {id}");

        var doc = _dbContext
                    .Documenten?
                    .Find(id);

        var result = (doc == null) ? "NOT Ok" : "Ok";
        _logger.LogInformation($"Get document with id {id} was {result}");

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

    public async Task SaveDocument(Document document)
    {
        _logger.LogDebug($"SaveAsync document merge {document.Name}");

        if (document.Id == 0) 
        {
            _dbContext.Documenten?.Add(document);
            _logger.LogInformation($"Document {document.Name} added");
        }
        else
        { 
            _dbContext.Documenten?.Update(document);
            _logger.LogInformation($"Document {document.Name} updated");
        }

        await _dbContext.SaveChangesAsync();
    }
}
