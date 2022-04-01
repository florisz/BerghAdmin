using Mailjet.Client.TransactionalEmails.Response;
using Microsoft.Extensions.Logging;

namespace BerghAdmin.ApplicationServices.Mail
{
    public class MailResponseHandlerService : IMailResponseHandlerService
    {
        private readonly ILogger<SendMailService> _logger;

        public MailResponseHandlerService(ILogger<SendMailService> logger)
        {
            _logger = logger;
        }

        public Task HandleMailResponseAsync(TransactionalEmailResponse response)
        {
            foreach (var messageResult in response.Messages)
            {
                foreach (var emailResult in messageResult.To)
                {
                    _logger.LogInformation("Message {messageUuid} - {messageHref}: status {status}, to {to}",
                        emailResult.MessageUUID, emailResult.MessageHref, messageResult.Status, emailResult.Email);
                }
            }
            return Task.CompletedTask;
        }
    }
}
