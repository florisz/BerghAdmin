using System;
using Mailjet.Client;
using Mailjet.Client.Resources;
using BerghAdmin.Services.Context;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace BerghAdmin.Services
{
    public class SendMailService : ISendMailService
    {
        IContextService _contextService;
        MailjetClient _mailJetClient;

        public SendMailService(IContextService contextService)
        {
            _contextService = contextService;
            var apiKey = _contextService.Context.MailJetConfiguration.ApiKey;
            var apiSecret = _contextService.Context.MailJetConfiguration.ApiSecret;
            _mailJetClient = new MailjetClient(apiKey, apiSecret);
        }

        public async Task SendMail()
        {
            var requestJson = "{\"Messages\":[" +
    				"{" +
						"\"From\": {" +
								"\"Email\": \"fzwarteveen@gmail.com\"," +
								"\"Name\": \"Floris\"" +
						"}," +
						"\"To\": [" +
								"{" +
										"\"Email\": \"fzwarteveen@gmail.com\"," +
										"\"Name\": \"Floris\"" +
								"}" +
						"]," +
						"\"Subject\": \"Greetings from Mailjet.\"," +
						"\"TextPart\": \"My first Mailjet email\"," +
						"\"HTMLPart\": \"<h3>Hi there, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!\"" +
	    			"}" +
		        "]" +
	        "}";
            // var request = new MailjetRequest { Resource = SendV31.Resource }
            //     .Property(Send.Messages, JToken.Parse(requestJson)); 
            var request = new MailjetRequest { Resource = SendV31.Resource }
                .Property(Send.Messages, new JArray 
                {
                    new JObject 
                    {
                        {
                            "From", new JObject {
                                {"Email", "fzwarteveen@gmail.com"}, 
                                {"Name", "Floris"}
                            }
                        }, 
                        {
                            "To", new JArray {
                                    new JObject {
                                        {"Email", "fzwarteveen@gmail.com" }, 
                                        {"Name", "Floris" }
                                    },
                            }
                        }, 
                        {
                            "Subject", "Greetings from Mailjet."
                        }, 
                        {
                            "TextPart", "My first Mailjet email"
                        }, 
                        {
                            "HTMLPart", "<h3>Hi there, welcome to <a href='https://www.mailjet.com/'>Mailjet</a>!</h3><br />May the delivery force be with you!"
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
}
