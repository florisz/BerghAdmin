using BerghAdmin.ApplicationServices.Mail;
using Mailjet.Client.TransactionalEmails;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework;

namespace BerghAdmin.Tests.MailTests
{
    [TestFixture]
    public class MailjetMappersTests
    {
        [Test]
        public void ToMailjetAddress_NullMailAddress_ShouldReturnEmptySendContact()
        {
            MailAddress? address = null;

            SendContact? actual = address.ToMailjetAddress();

            Assert.That(actual == null);
        }

        [Test]
        public void ToMailjetAddress_MailAddressWithoutName_ShouldReturnSendContact()
        {
            MailAddress? address = new("test@test.xyz", null);

            SendContact? actual = address.ToMailjetAddress();

            Assert.That(actual, !Is.EqualTo(null));
            Assert.That("test@test.xyz" == actual!.Email);
            Assert.That(null == actual!.Name);
        }

        [Test]
        public void ToMailjetAddress_MailAddressWithName_ShouldReturnSendContact()
        {
            MailAddress? address = new("test@test.xyz", "Test address");

            SendContact? actual = address.ToMailjetAddress();

            Assert.That(actual, !Is.EqualTo(null));
            Assert.That("test@test.xyz" == actual!.Email);
            Assert.That("Test address" == actual!.Name);
        }

        [Test]
        public void ToMailjetAddresses_NullMailAddressList_ShouldReturnEmptyList()
        {
            List<MailAddress>? addresses = null;

            var actual = addresses.ToMailjetAddresses();

            Assert.That(actual.IsNullOrEmpty());
        }

        [Test]
        public void ToMailjetAttachment_NullAttachment_ShouldReturnEmptyAttachment()
        {
            MailAttachment? attachment = null;

            Attachment? actual = attachment.ToMailjetAttachment();

            Assert.That(actual == null);
        }

        [Test]
        public void ToMailjetAttachment_Attachment_ShouldReturnAttachment()
        {
            MailAttachment? attachment = new("file.jpg", "image/jpeg", "abc", null);

            Attachment? actual = attachment.ToMailjetAttachment();

            Assert.That(actual, !Is.EqualTo(null));
            Assert.That(attachment.FilenameOnServer == actual!.Filename);
            Assert.That(attachment.ContentType == actual.ContentType);
            Assert.That(attachment.Base64Content == actual.Base64Content);
            Assert.That(attachment.ContentID == actual.ContentID);
        }


        [Test]
        public void ToMailjetAttachments_NullMailAttachmentList_ShouldReturnEmptyList()
        {
            List<MailAttachment>? attachments = null;

            var actual = attachments.ToMailjetAttachments();

            Assert.That(actual.IsNullOrEmpty());
        }

        [Test]
        public void ToMailjetMessage_NullMessage_ShouldReturnNoEmails()
        {
            MailMessage? message = null;

            IEnumerable<TransactionalEmail> actual = message.ToMailjetMessages();

            Assert.That(actual.IsNullOrEmpty());
        }

        [Test]
        public void ToMailjetMessage_MinimalMessage_ShouldReturnOneEmail()
        {
            MailMessage? message = new()
            {
                From = new MailAddress("sender@test.xyz", null),
                To = new() { new MailAddress("recipient@test.xyz", null) },
                Subject = "Subject",
                TextBody = "Contents"
            };

            var actual = message.ToMailjetMessages();

            Assert.That(1 == actual.Count());
            TransactionalEmail email = actual.First();
            Assert.That("sender@test.xyz" == email.From.Email);
            Assert.That(null == email.From.Name);
            Assert.That("recipient@test.xyz" == email.To.First().Email);
            Assert.That(null == email.To.First().Name);
            Assert.That("Subject" == email.Subject);
            Assert.That("Contents" == email.TextPart);
            Assert.That(string.Empty == email.HTMLPart);
        }

        [Test]
        public void ToMailjetMessage_FullMessage_ShouldReturnEmails()
        {
            MailMessage? message = new()
            {
                From = new MailAddress("sender@test.xyz", "Sender"),
                To = new()
                {
                    new MailAddress("recipient1@test.xyz", "Recipient 1"),
                    new MailAddress("recipient2@test.xyz", "Recipient 2")
                },
                Cc = new()
                {
                    new MailAddress("cc1@test.xyz", "Copy 1"),
                    new MailAddress("cc2@test.xyz", "Copy 2")
                },
                Bcc = new()
                {
                    new MailAddress("bcc1@test.xyz", "BlindCopy 1"),
                    new MailAddress("bcc2@test.xyz", "BlindCopy 2")
                },
                Subject = "Subject",
                TextBody = "Contents",
                HtmlBody = "<h1>HTML</h1><p>Text</p><img src=\"images/test.jpg\" />",
                InlinedAttachments = new()
                {
                    new MailAttachment("test.jpg", "image/jpeg", "abc", "id01")
                }
            };

            IEnumerable<TransactionalEmail> actual = message.ToMailjetMessages();

            var emails = actual.ToList();
            Assert.That(2 == emails.Count);
            TransactionalEmail firstEmail = emails[0];
            Assert.That("sender@test.xyz" == firstEmail.From.Email);
            Assert.That("Sender" == firstEmail.From.Name);
            Assert.That("recipient1@test.xyz" == firstEmail.To.First().Email);
            Assert.That("Recipient 1" == firstEmail.To.First().Name);
            Assert.That("Subject" == firstEmail.Subject);
            Assert.That("Contents" == firstEmail.TextPart);
            Assert.That("<h1>HTML</h1><p>Text</p><img src=\"images/test.jpg\" />" == firstEmail.HTMLPart);
            Assert.That(2 == firstEmail.Cc.Count);
            Assert.That(2 == firstEmail.Bcc.Count);
            Assert.That(1 == firstEmail.InlinedAttachments.Count);
            TransactionalEmail secondEmail = emails[1];
            Assert.That("sender@test.xyz" == secondEmail.From.Email);
            Assert.That("Sender" == secondEmail.From.Name);
            Assert.That("recipient2@test.xyz" == secondEmail.To.First().Email);
            Assert.That("Recipient 2" == secondEmail.To.First().Name);
            Assert.That("Subject" == secondEmail.Subject);
            Assert.That("Contents" == secondEmail.TextPart);
            Assert.That("<h1>HTML</h1><p>Text</p><img src=\"images/test.jpg\" />" == secondEmail.HTMLPart);
            Assert.That(2 == secondEmail.Cc.Count);
            Assert.That(2 == secondEmail.Bcc.Count);
            Assert.That(1 == secondEmail.InlinedAttachments.Count);
        }
    }
}
