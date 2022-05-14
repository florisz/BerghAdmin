using Mailjet.Client.TransactionalEmails;

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

            return addresses.Select(a => a.ToMailjetAddress()!);
        }

        public static Attachment? ToMailjetAttachment(this MailAttachment? attachment)
        {
            if (attachment is null)
            {
                return null;
            }

            return new Attachment(attachment.FilenameOnServer, attachment.ContentType, attachment.Base64Content, attachment.ContentID);
        }

        public static IEnumerable<Attachment> ToMailjetAttachments(this IEnumerable<MailAttachment>? attachments)
        {
            if (attachments is null)
            {
                return Array.Empty<Attachment>();
            }

            return attachments.Select(a => a.ToMailjetAttachment()!);
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
                    .WithInlinedAttachments(mailMessage.InlinedAttachments.ToMailjetAttachments())
                    .Build();
                emails.Add(email);
            }

            return emails;
        }
    }
}
