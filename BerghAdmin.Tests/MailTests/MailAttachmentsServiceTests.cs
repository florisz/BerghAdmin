using BerghAdmin.ApplicationServices.Mail;

using FluentAssertions;

using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

using System.IO.Abstractions.TestingHelpers;
using System.Text.RegularExpressions;

namespace BerghAdmin.Tests.MailTests
{
    [TestFixture]
    public class MailAttachmentsServiceTests
    {
        [TestCase(null, null)]
        [TestCase("", "")]
        [TestCase("<p>Message</p>", "<p>Message</p>")]
        [TestCase("Logo: <img />", "Logo: <img />")]
        [TestCase("Logo: <img src=\"\" />", "Logo: <img src=\"\" />")]
        [TestCase("Logo: <img src=\"http://web.xyz/img.jpg\" />", "Logo: <img src=\"http://web.xyz/img.jpg\" />")]
        [TestCase("Logo: <img src=\"images/missing.jpg\" />", "Logo: <img src=\"images/missing.jpg\" />")]
        public async Task AddInlinedAttachments(string? body, string? expected)
        {
            var message = await ReplaceImage(body);
            message.HtmlBody.Should().Be(expected);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithServerImage_ShouldReplaceWithInlinedAttachment()
        {
            var message = await ReplaceImage("Logo: <img src=\"images/LogoBihz.jpg\" />");

            StringAssert.IsMatch(_imageWithContentIdPattern, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithSameImageTwice_ShouldReplaceWithInlinedAttachment()
        {
            var message = await ReplaceImage("<div><p>Logo: <img src=\"images/LogoBihz.jpg\" /></p><p>Logo copy: <img src=\"images/logoBIHZ.JPG\" /></p></div>");

            Assert.AreEqual(2, Regex.Matches(message.HtmlBody, _imageWithContentIdPattern).Count);
        }

        private static MailAttachmentsService CreateService()
        {
            var mockFiles = new Dictionary<string, MockFileData>()
            {
                { @"/images/LogoBihz.jpg", new MockFileData(new byte[] { 1, 2, 3 } ) }
            };

            return new MailAttachmentsService(
                @"/",
                new MockFileSystem(mockFiles),
                Mock.Of<ILogger<MailAttachmentsService>>());
        }

        private static async Task<MailMessage> ReplaceImage(string htmlBody)
        {
            var message = new MailMessage()
            {
                HtmlBody = htmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);
            return message;
        }

        private const string _imageWithContentIdPattern = "<img src=\"cid:[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\"\\s?\\/?>";
    }
}
