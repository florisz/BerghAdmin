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
    public int Id { get; set; }
    public string Name { get; set; }
    public ContentTypeEnum ContentType { get; set; }
    public TemplateTypeEnum TemplateType { get; set;}
    public byte[] Content { get; set; }
    public bool IsMergeTemplate { get; set; }
    public string Owner { get; set;}
}
