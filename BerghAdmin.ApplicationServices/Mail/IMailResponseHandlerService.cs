using Mailjet.Client.TransactionalEmails.Response;

namespace BerghAdmin.ApplicationServices.Mail
{
    public interface IMailResponseHandlerService
    {
        Task HandleMailResponseAsync(TransactionalEmailResponse response);
    }
}