using Newtonsoft.Json.Linq;

namespace BerghAdmin.ApplicationServices.Mail
{
    public static class MailjetMappers
    {
        public static JObject ToMailjetAddress(this MailAddress? address)
        {
            if (address is null)
            {
                return new JObject();
            }

            return new JObject
            {
                { "Email", address.Address },
                { "Name", address.Name }
            };
        }

        public static JArray ToMailjetAddresses(this IEnumerable<MailAddress>? addresses)
        {
            if (addresses is null)
            {
                return new JArray();
            }

            return new JArray(addresses.Select(address => address.ToMailjetAddress()));
        }

        public static JObject ToMailjetMessage(this MailMessage? message)
        {
            if (message is null)
            {
                return new JObject();
            }

            JObject result = new()
            {
                {
                    "From",
                    message.From.ToMailjetAddress()
                },
                {
                    "To",
                    message.To.ToMailjetAddresses()
                },
                {
                    "Cc",
                    message.Cc.ToMailjetAddresses()
                },
                {
                    "Bcc",
                    message.Bcc.ToMailjetAddresses()
                },
                {
                    "Subject",
                    message.Subject
                },
                {
                    "TextPart",
                    message.TextBody
                },
                {
                    "HTMLPart",
                    message.HtmlBody
                }
            };

            return result;
        }

        public static JArray ToMailjetMessages(this IEnumerable<MailMessage>? messages)
        {
            if (messages is null)
            {
                return new JArray();
            }

            return new JArray(messages.Select(message => message.ToMailjetMessage()));
        }
    }
}
