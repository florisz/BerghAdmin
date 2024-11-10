using BerghAdmin.ApplicationServices.DocIO;

using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;

namespace BerghAdmin.Services.Documenten;

public class DocumentMergeService : IDocumentMergeService
{
    private readonly ILogger<DocumentMergeService> _logger;
    private readonly IDocumentService _documentService;

    public DocumentMergeService(IDocumentService documentService, ILogger<DocumentMergeService> logger)
    {
        _documentService = documentService;
        _logger = logger;
    }

    public void DeleteMergeTemplate(int id)
    {
        throw new NotImplementedException();
    }

    public Document? GetMergeTemplateById(int id)
    {
        var doc = _documentService.GetDocumentById(id);
        if (doc == null)
        {
            return null;
        }

        if (!doc.IsMergeTemplate)
        {
            throw new ArgumentException($"document with name {doc.Name} is not a mergetemplate");
        }

        return doc;
    }

    public Document? GetMergeTemplateByName(string name)
    {
        var doc = _documentService.GetDocumentByName(name);
        if (doc == null)
        {
            return null;
        }

        if (!doc.IsMergeTemplate)
        {
            throw new ArgumentException($"document with name {doc.Name} is not a mergetemplate");
        }

        return doc;
    }

    public List<Document> GetMergeTemplates()
    {
        throw new NotImplementedException();
    }

    public Stream Merge(MemoryStream template, Dictionary<string, string> mergeFields)
    {
        _logger.LogDebug("Entering document merge {mergefields}", mergeFields);

        var fieldNames = mergeFields.Keys.ToArray();
        var fieldValues = mergeFields.Values.ToArray();

        var wordDocument = new WordDocument(template, FormatType.Docx);
        wordDocument.MailMerge.Execute(fieldNames, fieldValues);

        var outputStream = new MemoryStream();
        wordDocument.Save(outputStream, FormatType.Docx);

        _logger.LogInformation("Document merged succesfully, document stream wit size {size} is returned", outputStream.Length);

        return outputStream;
    }

    public void SaveMergeTemplate(Document mergeTemplate)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<string> GetMergeFieldsFor(Document document)
    {
        if (document.DocumentType != DocumentTypeEnum.Word)
        {
            throw new ApplicationException($"Document with name {document.Name} is not a Word document.");
        }
        if (document.Content == null)
        {
            throw new ApplicationException($"Document with name {document.Name} has no content.");
        }
        return DocIOInterface.GetMergeFields(new MemoryStream(document.Content));
    }

}
