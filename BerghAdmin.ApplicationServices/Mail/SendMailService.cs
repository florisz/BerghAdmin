using Mailjet.Client;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Logging;

namespace BerghAdmin.ApplicationServices.Mail;

public class SendMailService : ISendMailService
{
    private readonly IMailjetClient _mailjetClient;
    private readonly IMailResponseHandlerService _mailResponseHandlerService;
    private readonly ILogger<SendMailService> _logger;

    public SendMailService(
        IMailjetClient mailjetClient,
        IMailResponseHandlerService mailResponseHandlerService,
        ILogger<SendMailService> logger)
    {
        _mailjetClient = mailjetClient;
        _mailResponseHandlerService = mailResponseHandlerService;
        _logger = logger;
    }

    public async Task SendMailAsync(MailMessage message, bool isSandboxMode = false)
    {
        Dictionary<string, List<string>> validationProblems = message.Validate();
        if (validationProblems.Any())
        {
            Console.WriteLine(validationProblems);
            return;
        }

        var emails = message.ToMailjetMessages();
        var response = await _mailjetClient.SendTransactionalEmailsAsync(emails, isSandboxMode);

        await _mailResponseHandlerService.HandleMailResponseAsync(response);
    }
}
