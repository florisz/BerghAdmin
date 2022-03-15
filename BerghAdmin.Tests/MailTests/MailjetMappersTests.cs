using BerghAdmin.ApplicationServices.Mail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace BerghAdmin.Tests.MailTests
{
    [TestFixture]
    public class MailjetMappersTests
    {
        [Test]
        public void ToMailjetAddress_NullMailAddress_ShouldReturnEmptyJObject()
        {
            MailAddress? address = null;

            JObject actual = address.ToMailjetAddress();

            string expected = "{}";
            Assert.AreEqual(expected, actual.ToString(Formatting.None));
        }

        [Test]
        public void ToMailjetAddress_MailAddressWithoutName_ShouldReturnJObject()
        {
            MailAddress? address = new("test@test.xyz", null);

            JObject actual = address.ToMailjetAddress();

            string expected = "{\"Email\":\"test@test.xyz\",\"Name\":null}";
            Assert.AreEqual(expected, actual.ToString(Formatting.None));
        }

        [Test]
        public void ToMailjetAddress_MailAddressWithName_ShouldReturnJObject()
        {
            MailAddress? address = new("test@test.xyz", "Test address");

            JObject actual = address.ToMailjetAddress();

            string expected = "{\"Email\":\"test@test.xyz\",\"Name\":\"Test address\"}";
            Assert.AreEqual(expected, actual.ToString(Formatting.None));
        }

        [Test]
        public void ToMailjetMessage_NullMessage_ShouldReturnEmptyJObject()
        {
            MailMessage? message = null;

            JObject actual = message.ToMailjetMessage();

            string expected = "{}";
            Assert.AreEqual(expected, actual.ToString(Formatting.None));
        }

        [Test]
        public void ToMailjetMessage_SparseMessage_ShouldReturnJObject()
        {
            MailMessage? message = new()
            {
                From = new MailAddress("sender@test.xyz", null),
                To = new() { new MailAddress("recipient@test.xyz", null) },
                Subject = "Subject",
                TextBody = "Contents"
            };

            JObject actual = message.ToMailjetMessage();

            string expected = "{\"From\":{\"Email\":\"sender@test.xyz\",\"Name\":null},\"To\":[{\"Email\":\"recipient@test.xyz\",\"Name\":null}],\"Cc\":[],\"Bcc\":[],\"Subject\":\"Subject\",\"TextPart\":\"Contents\",\"HTMLPart\":null}";
            Assert.AreEqual(expected, actual.ToString(Formatting.None));
        }

        [Test]
        public void ToMailjetMessage_FullMessage_ShouldReturnJObject()
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
                HtmlBody = "<h1>HTML</h1><p>Text</p>"
            };

            JObject actual = message.ToMailjetMessage();

            string expected = "{\"From\":{\"Email\":\"sender@test.xyz\",\"Name\":\"Sender\"},\"To\":[{\"Email\":\"recipient1@test.xyz\",\"Name\":\"Recipient 1\"},{\"Email\":\"recipient2@test.xyz\",\"Name\":\"Recipient 2\"}],\"Cc\":[{\"Email\":\"cc1@test.xyz\",\"Name\":\"Copy 1\"},{\"Email\":\"cc2@test.xyz\",\"Name\":\"Copy 2\"}],\"Bcc\":[{\"Email\":\"bcc1@test.xyz\",\"Name\":\"BlindCopy 1\"},{\"Email\":\"bcc2@test.xyz\",\"Name\":\"BlindCopy 2\"}],\"Subject\":\"Subject\",\"TextPart\":\"Contents\",\"HTMLPart\":\"<h1>HTML</h1><p>Text</p>\"}";
            Assert.AreEqual(expected, actual.ToString(Formatting.None));
        }
    }
}
