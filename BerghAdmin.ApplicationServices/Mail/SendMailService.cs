using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;

using Microsoft.Extensions.Logging;

namespace BerghAdmin.ApplicationServices.Mail;

public class SendMailService : ISendMailService
{
    private readonly IMailjetClient mailjetClient;
    private readonly ILogger<SendMailService> logger;

    public SendMailService(
        IMailjetClient mailjetClient,
        ILogger<SendMailService> logger)
    {
        this.mailjetClient = mailjetClient;
        this.logger = logger;
    }

    public async Task SendMail(MailMessage message, bool isSandboxMode = false)
    {
        var validationProblems = message.Validate();
        if (validationProblems.Any())
        {
            logger.LogError("Error in email: {problems}", validationProblems);
            return;
        }

        var emails = message.ToMailjetMessages();
        var response = await mailjetClient.SendTransactionalEmailsAsync(emails, isSandboxMode);

        response.Messages
            .SelectMany(m => m.To.Select(t => new { m.Status, t.MessageUUID, t.MessageHref, t.Email }))
            .ToList()
            .ForEach(m => logger.LogInformation("Message {messageUuid} - {messageHref}: status {status}, to {to}",
                    m.MessageUUID, m.MessageHref, m.Status, m.Email)
            );
    }

    public async Task SendMail(string to, string from, string subject, string message)
    {
        var msg = new MailMessage
        {
            From = new MailAddress(from, from),
            To = new() { new MailAddress(to, to) },
            Subject = subject,
            TextBody = message
        };

        await SendMail(msg, false);
    }
}
