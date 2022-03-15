using BerghAdmin.ApplicationServices.Mail;
using NUnit.Framework;

namespace BerghAdmin.Tests.MailTests
{
    [TestFixture]
    public class MailValidatorTests
    {
        [Test]
        public void Validate_NoSubject_ShouldReportSubjectRequired()
        {
            MailMessage message = new()
            {
                From = new MailAddress("test@berghinhetzadel.nl", null),
                To = new() { new MailAddress("recipient@test.xyz", null) }
            };

            var actual = message.Validate();

            Assert.That(actual.ContainsKey("Subject"));
            Assert.That(actual["Subject"].Count == 1);
        }

        [Test]
        public void Validate_NoSender_ShouldReportSenderRequired()
        {
            MailMessage message = new()
            {
                To = new() { new MailAddress("recipient@test.xyz", null) },
                Subject = "Test"
            };

            var actual = message.Validate();

            Assert.That(actual.ContainsKey("From"));
            Assert.That(actual["From"].Count == 1);
        }

        [Test]
        public void Validate_SenderDomainNotBihz_ShouldReportWrongDomain()
        {
            MailMessage message = new()
            {
                From = new MailAddress("test@bad.xyz", null),
                To = new() { new MailAddress("recipient@test.xyz", null) },
                Subject = "Test"
            };

            var actual = message.Validate();

            Assert.That(actual.ContainsKey("From"));
            Assert.That(actual["From"].Count == 1);
        }

        [Test]
        public void Validate_NoRecipients_ShouldReportRecipientRequired()
        {
            MailMessage message = new()
            {
                From = new MailAddress("test@berghinhetzadel.nl", null),
                Subject = "Test"
            };

            var actual = message.Validate();

            Assert.That(actual.ContainsKey("To"));
            Assert.That(actual["To"].Count == 1);
        }

        [Test]
        public void Validate_ValidMessage_ShouldReportNoValidationProblems()
        {
            MailMessage message = new()
            {
                From = new MailAddress("test@berghinhetzadel.nl", null),
                To = new() { new MailAddress("recipient@test.xyz", null) },
                Subject = "Test"
            };

            var actual = message.Validate();

            Assert.IsEmpty(actual);
        }
    }
}
