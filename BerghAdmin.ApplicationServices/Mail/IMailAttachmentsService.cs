namespace BerghAdmin.ApplicationServices.Mail
{
    public interface IMailAttachmentsService
    {
        Task ReplaceServerImagesWithInlinedAttachmentsAsync(MailMessage message);
    }
}