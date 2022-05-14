using BerghAdmin.ApplicationServices.Mail;

using FluentAssertions;

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

            actual.Should().ContainKey("Subject");
            actual["Subject"].Message.Should().Be("Subject is required");
        }

        [Test]
        public void Validate_NoSender_ShouldReportSenderRequired()
        {
            MailMessage message = new()
            {
                Subject = "Test",
                To = new() { new MailAddress("recipient@test.xyz", null) },
            };

            var actual = message.Validate();

            actual.Should().ContainKey("From");
            actual["From"].Message.Should().Be("Sender is required");
        }

        [Test]
        public void Validate_SenderDomainNotBihz_ShouldReportWrongDomain()
        {
            MailMessage message = new()
            {
                From = new MailAddress("test@bad.xyz", null),
                Subject = "Test",
                To = new() { new MailAddress("recipient@test.xyz", null) },
            };

            var actual = message.Validate();

            actual.Should().ContainKey("Address");
            actual["Address"].Message.Should().Contain("Domain of sender");
        }

        [Test]
        public void Validate_NoRecipients_ShouldReportRecipientRequired()
        {
            MailMessage message = new()
            {
                From = new MailAddress("test@berghinhetzadel.nl", null),
                Subject = "Test",
            };

            var actual = message.Validate();

            actual.Should().ContainKey("To");
            actual["To"].Message.Should().Be("At least one To address must be specified");
        }

        [Test]
        public void Validate_ValidMessage_ShouldReportNoValidationProblems()
        {
            MailMessage message = new()
            {
                From = new MailAddress("test@berghinhetzadel.nl", null),
                Subject = "Test",
                To = new() { new MailAddress("recipient@test.xyz", null) },
            };

            message.Validate().Should().BeEmpty();
        }
    }
}
