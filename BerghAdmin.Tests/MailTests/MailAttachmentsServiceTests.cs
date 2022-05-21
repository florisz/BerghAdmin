using System.IO.Abstractions.TestingHelpers;
using System.Text.RegularExpressions;
using BerghAdmin.ApplicationServices.Mail;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace BerghAdmin.Tests.MailTests
{
    [TestFixture]
    public class MailAttachmentsServiceTests
    {
        [Test]
        public async Task AddInlinedAttachments_NoHtmlBody_ShouldReturnNull()
        {
            var message = new MailMessage()
            {
                HtmlBody = null!
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.IsNull(message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_EmptyHtmlBody_ShouldReturnEmptyHtmlBody()
        {
            var expectedHtmlBody = "";
            var message = new MailMessage()
            {
                HtmlBody = expectedHtmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.AreEqual(expectedHtmlBody, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithoutServerImages_ShouldReturnSameHtmlBody()
        {
            var expectedHtmlBody = "<p>Message</p>";
            var message = new MailMessage()
            {
                HtmlBody = expectedHtmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.AreEqual(expectedHtmlBody, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithImageWithoutSource_ShouldReturnSameHtmlBody()
        {
            var expectedHtmlBody = "Logo: <img />";
            var message = new MailMessage()
            {
                HtmlBody = expectedHtmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.AreEqual(expectedHtmlBody, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithEmptyImageSource_ShouldReturnSameHtmlBody()
        {
            var expectedHtmlBody = "Logo: <img src=\"\" />";
            var message = new MailMessage()
            {
                HtmlBody = expectedHtmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.AreEqual(expectedHtmlBody, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithInternetImage_ShouldReturnSameHtmlBody()
        {
            var expectedHtmlBody = "Logo: <img src=\"http://web.xyz/img.jpg\" />";
            var message = new MailMessage()
            {
                HtmlBody = expectedHtmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.AreEqual(expectedHtmlBody, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithNotExistingServerImage_ShouldReturnSameHtmlBody()
        {
            var expectedHtmlBody = "Logo: <img src=\"images/missing.jpg\" />";
            var message = new MailMessage()
            {
                HtmlBody = expectedHtmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            StringAssert.StartsWith(expectedHtmlBody, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithServerImage_ShouldReplaceWithInlinedAttachment()
        {
            var htmlBody = "Logo: <img src=\"images/LogoBihz.jpg\" />";
            var message = new MailMessage()
            {
                HtmlBody = htmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            StringAssert.IsMatch(_imageWithContentIdPattern, message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithSameImageTwice_ShouldReplaceWithInlinedAttachment()
        {
            var htmlBody = "<div><p>Logo: <img src=\"images/LogoBihz.jpg\" /></p><p>Logo copy: <img src=\"images/logoBIHZ.JPG\" /></p></div>";
            var message = new MailMessage()
            {
                HtmlBody = htmlBody
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.AreEqual(2, Regex.Matches(message.HtmlBody, _imageWithContentIdPattern).Count);
        }

        private static MailAttachmentsService CreateService()
        {
            var mockFiles = new Dictionary<string, MockFileData>()
            {
                { @"C:\images\LogoBihz.jpg", new MockFileData(new byte[] { 1, 2, 3 } ) }
            };
            var fileSystem = new MockFileSystem(mockFiles);

            var mockLogger = new Mock<ILogger<MailAttachmentsService>>();

            var cacheOptions = Options.Create(new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(10)
            });
            var cache = new MemoryCache(cacheOptions);

            return new MailAttachmentsService(@"C:\", fileSystem, cache, mockLogger.Object);
        }

        private const string _imageWithContentIdPattern = "<img src=\"cid:[0-9A-Fa-f]{8}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{4}[-][0-9A-Fa-f]{12}\"\\s?\\/?>";
    }
}
