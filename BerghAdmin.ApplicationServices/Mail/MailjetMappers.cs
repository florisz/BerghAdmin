using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Newtonsoft.Json.Linq;

namespace BerghAdmin.ApplicationServices.Mail
{
    public static class MailjetMappers
    {
        public static SendContact? ToMailjetAddress(this MailAddress? address)
        {
            if (address is null)
            {
                return null;
            }

            return new(address.Address, address.Name);
        }

        public static IEnumerable<SendContact> ToMailjetAddresses(this IEnumerable<MailAddress>? addresses)
        {
            if (addresses is null)
            {
                return Array.Empty<SendContact>();
            }

            return addresses.Select(a => new SendContact(a.Address, a.Name));
        }

        public static IEnumerable<TransactionalEmail> ToMailjetMessages(this MailMessage? mailMessage)
        {
            if (mailMessage is null)
            {
                return Array.Empty<TransactionalEmail>();
            }

            var emails = new List<TransactionalEmail>();

            foreach (var toAddress in mailMessage.To)
            {
                var email = new TransactionalEmailBuilder()
                    .WithFrom(mailMessage.From.ToMailjetAddress())
                    .WithTo(toAddress.ToMailjetAddress())
                    .WithCc(mailMessage.Cc.ToMailjetAddresses())
                    .WithBcc(mailMessage.Bcc.ToMailjetAddresses())
                    .WithSubject(mailMessage.Subject)
                    .WithTextPart(mailMessage.TextBody)
                    .WithHtmlPart(mailMessage.HtmlBody)
                    .Build();
                emails.Add(email);
            }

            return emails;
        }
    }
}
