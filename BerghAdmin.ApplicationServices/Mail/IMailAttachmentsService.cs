namespace BerghAdmin.ApplicationServices.Mail
{
    public interface IMailAttachmentsService
    {
        void ReplaceServerImagesWithInlinedAttachments(MailMessage message);
    }
}