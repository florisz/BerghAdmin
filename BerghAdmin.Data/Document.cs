#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
namespace BerghAdmin.Data;

public enum ContentTypeEnum
{
    Word,
    Excel,
    Html,
    Image,
    Text,
    Pdf
}
    
public enum TemplateTypeEnum
{
    Ambassadeur,
    Fietser,
    Golfer,
    Algemeen, 
    None
}

public class Document
{
    private readonly List<string>? _mergeFields = null;

    public int Id { get; set; }
    public string Name { get; set; }
    public ContentTypeEnum ContentType { get; set; }
    public TemplateTypeEnum TemplateType { get; set;}
    public byte[] Content { get; set; }
    public bool IsMergeTemplate { get; set; }
    public string Owner { get; set;}
         
    public List<string> GetMergeFields()
    {
        //if (_mergeFields == null)
        //{
        //    if (this.ContentType != ContentTypeEnum.Word)
        //    {
        //        throw new ApplicationException($"Document with name {this.Name} is not a Word document.");
        //    }
        //    if (this.Content == null)
        //    {
        //        throw new ApplicationException($"Document with name {this.Name} has no content.");
        //    }
        //    _mergeFields = DocIOInterface.GetMergeFields(new MemoryStream(this.Content));
        //}
        return _mergeFields;
    }
}
