using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace BerghAdmin.ApplicationServices.Mail;

public class SendMailService : ISendMailService
{
    private readonly IMailjetClient _mailjetClient;
    private readonly ILogger<SendMailService> _logger;

    public SendMailService(
        IMailjetClient mailjetClient,
        ILogger<SendMailService> logger)
    {
        _mailjetClient = mailjetClient;
        _logger = logger;
    }

    public async Task<bool> SendMail(MailMessage message)
    {
        Dictionary<string, List<string>> validationProblems = message.Validate();
        if (validationProblems.Any())
        {
            Console.WriteLine(validationProblems);
            return false;
        }

        List<MailMessage> mailMessages = new() { message };
        var mailjetMessages = mailMessages.ToMailjetMessages();
        var request = new MailjetRequest { Resource = SendV31.Resource }
            .Property(Send.Messages, mailjetMessages);

        var response = await _mailjetClient.PostAsync(request);
        bool success = response.IsSuccessStatusCode;
        string data = response.GetData().ToString(Formatting.Indented);
        if (success)
        {
            _logger.LogInformation("Mail was successfully sent.\nTotal: {total}, Count: {count}\nData:\n{data}",
                response.GetTotal(), response.GetCount(), data);
        }
        else
        {
            _logger.LogError("Error sending mail.\nStatusCode: {statusCode}\nErrorInfo: {errorInfo}\nErrorMessage: {errorMessage}\nData:\n{data}",
                response.StatusCode, response.GetErrorInfo(), response.GetErrorMessage(), data);
        }

        return success;
    }
}
