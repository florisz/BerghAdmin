namespace BerghAdmin.ApplicationServices.Mail
{
    public record MailAttachment
    (
        string FilenameOnServer,
        string ContentType,
        string Base64Content,
        string? ContentID
    );
}
