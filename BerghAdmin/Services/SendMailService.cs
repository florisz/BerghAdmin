using Mailjet.Client;
using Mailjet.Client.Resources;
using Newtonsoft.Json.Linq;
using Microsoft.Extensions.Options;
using BerghAdmin.Services.Configuration;

namespace BerghAdmin.Services;

public class SendMailService : ISendMailService
{
    MailjetClient _mailJetClient;

    public SendMailService(IOptions<MailJetConfiguration> settings)
    {
        var apiKey = settings.Value.ApiKey;
        var apiSecret = settings.Value.ApiSecret;
        _mailJetClient = new MailjetClient(apiKey, apiSecret);
    }

    public async Task SendMail(string mailBody)
    {
        var request = new MailjetRequest { Resource = SendV31.Resource }
            .Property(Send.Messages, new JArray 
            {
                new JObject 
                {
                    {
                        "From", new JObject {
                            {"Email", "fzwarteveen@gmail.com"}, 
                            {"Name", "Floris Zwarteveen"}
                        }
                    }, 
                    {
                        "To", new JArray {
                                new JObject {
                                    {"Email", "lpreumer@me.com" }, 
                                    {"Name", "Lars Peter Reumer" }
                                },
                                new JObject {
                                    {"Email", "secretaris@berghinhetzadel.nl" }, 
                                    {"Name", "Secretariaat" }
                                },
                                new JObject {
                                    {"Email", "fzwarteveen@gmail.com" }, 
                                    {"Name", "Floris Zwarteveen" }
                                },
                        }
                    }, 
                    {
                        "Subject", "Test mail nieuwe admin site"
                    }, 
                    {
                        "TextPart", "no text"
                    }, 
                    {
                        "HTMLPart", mailBody
                    }
                }
            });

        var response = await _mailJetClient.PostAsync(request);
        if (response.IsSuccessStatusCode) 
        {
            Console.WriteLine(string.Format("Total: {0}, Count: {1}\n", response.GetTotal(), response.GetCount()));
            Console.WriteLine(response.GetData());
        } 
        else 
        {
            Console.WriteLine(string.Format("StatusCode: {0}\n", response.StatusCode));
            Console.WriteLine(string.Format("ErrorInfo: {0}\n", response.GetErrorInfo()));
            Console.WriteLine(response.GetData());
            Console.WriteLine(string.Format("ErrorMessage: {0}\n", response.GetErrorMessage()));
        }
    }

}
