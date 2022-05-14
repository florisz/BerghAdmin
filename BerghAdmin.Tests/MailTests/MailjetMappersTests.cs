using BerghAdmin.ApplicationServices.Mail;
using Mailjet.Client.TransactionalEmails;
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

            Assert.IsNull(actual);
        }

        [Test]
        public void ToMailjetAddress_MailAddressWithoutName_ShouldReturnSendContact()
        {
            MailAddress? address = new("test@test.xyz", null);

            SendContact? actual = address.ToMailjetAddress();

            Assert.IsNotNull(actual);
            Assert.AreEqual("test@test.xyz", actual!.Email);
            Assert.AreEqual(null, actual!.Name);
        }

        [Test]
        public void ToMailjetAddress_MailAddressWithName_ShouldReturnSendContact()
        {
            MailAddress? address = new("test@test.xyz", "Test address");

            SendContact? actual = address.ToMailjetAddress();

            Assert.IsNotNull(actual);
            Assert.AreEqual("test@test.xyz", actual!.Email);
            Assert.AreEqual("Test address", actual!.Name);
        }

        [Test]
        public void ToMailjetAttachment_NullAttachment_ShouldReturnEmptyAttachment()
        {
            MailAttachment? attachment = null;

            Attachment? actual = attachment.ToMailjetAttachment();

            Assert.IsNull(actual);
        }

        [Test]
        public void ToMailjetAttachment_Attachment_ShouldReturnAttachment()
        {
            MailAttachment? attachment = new("file.jpg", "image/jpeg", "abc", null);

            Attachment? actual = attachment.ToMailjetAttachment();

            Assert.IsNotNull(actual);
            Assert.AreEqual(attachment.FilenameOnServer, actual.Filename);
            Assert.AreEqual(attachment.ContentType, actual.ContentType);
            Assert.AreEqual(attachment.Base64Content, actual.Base64Content);
            Assert.AreEqual(attachment.ContentID, actual.ContentID);
        }

        [Test]
        public void ToMailjetMessage_NullMessage_ShouldReturnNoEmails()
        {
            MailMessage? message = null;

            IEnumerable<TransactionalEmail> actual = message.ToMailjetMessages();

            Assert.IsEmpty(actual);
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

            Assert.AreEqual(1, actual.Count());
            TransactionalEmail email = actual.First();
            Assert.AreEqual("sender@test.xyz", email.From.Email);
            Assert.AreEqual(null, email.From.Name);
            Assert.AreEqual("recipient@test.xyz", email.To.First().Email);
            Assert.AreEqual(null, email.To.First().Name);
            Assert.AreEqual("Subject", email.Subject);
            Assert.AreEqual("Contents", email.TextPart);
            Assert.AreEqual(string.Empty, email.HTMLPart);
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
            Assert.AreEqual(2, emails.Count);
            TransactionalEmail firstEmail = emails[0];
            Assert.AreEqual("sender@test.xyz", firstEmail.From.Email);
            Assert.AreEqual("Sender", firstEmail.From.Name);
            Assert.AreEqual("recipient1@test.xyz", firstEmail.To.First().Email);
            Assert.AreEqual("Recipient 1", firstEmail.To.First().Name);
            Assert.AreEqual("Subject", firstEmail.Subject);
            Assert.AreEqual("Contents", firstEmail.TextPart);
            Assert.AreEqual("<h1>HTML</h1><p>Text</p><img src=\"images/test.jpg\" />", firstEmail.HTMLPart);
            Assert.AreEqual(2, firstEmail.Cc.Count);
            Assert.AreEqual(2, firstEmail.Bcc.Count);
            Assert.AreEqual(1, firstEmail.InlinedAttachments.Count);
            TransactionalEmail secondEmail = emails[1];
            Assert.AreEqual("sender@test.xyz", secondEmail.From.Email);
            Assert.AreEqual("Sender", secondEmail.From.Name);
            Assert.AreEqual("recipient2@test.xyz", secondEmail.To.First().Email);
            Assert.AreEqual("Recipient 2", secondEmail.To.First().Name);
            Assert.AreEqual("Subject", secondEmail.Subject);
            Assert.AreEqual("Contents", secondEmail.TextPart);
            Assert.AreEqual("<h1>HTML</h1><p>Text</p><img src=\"images/test.jpg\" />", secondEmail.HTMLPart);
            Assert.AreEqual(2, secondEmail.Cc.Count);
            Assert.AreEqual(2, secondEmail.Bcc.Count);
            Assert.AreEqual(1, secondEmail.InlinedAttachments.Count);
        }
    }
}
