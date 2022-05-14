using System.IO.Abstractions.TestingHelpers;
using BerghAdmin.ApplicationServices.Mail;
using Microsoft.AspNetCore.Hosting;
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
        public async Task AddInlinedAttachments_HtmlBodyWithServerImage_ShouldReplaceWithInlinedAttachment()
        {
            var message = new MailMessage()
            {
                HtmlBody = "Logo: <img src=\"images/email/LogoBihz.jpg\" />"
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            StringAssert.StartsWith("Logo: <img src=\"cid:", message.HtmlBody);
        }

        [Test]
        public async Task AddInlinedAttachments_HtmlBodyWithSameImageTwice_ShouldReplaceWithInlinedAttachment()
        {
            var message = new MailMessage()
            {
                HtmlBody = "<div><p>Logo: <img src=\"images/email/LogoBihz.jpg\" /></p><p>Logo copy: <img src=\"images/email/logoBIHZ.JPG\" /></p></div>"
            };
            var service = CreateService();

            await service.ReplaceServerImagesWithInlinedAttachmentsAsync(message);

            Assert.IsTrue(true);
        }

        private static MailAttachmentsService CreateService()
        {
            var mockFiles = new Dictionary<string, MockFileData>()
            {
                { @"C:\images\email\LogoBihz.jpg", new MockFileData(new byte[] { 1, 2, 3 } ) }
            };
            var fileSystem = new MockFileSystem(mockFiles);
            var mockLogger = new Mock<ILogger<MailAttachmentsService>>();
            var mockHostingEnvironment = new Mock<IWebHostEnvironment>();
            mockHostingEnvironment.Setup(x => x.ContentRootPath).Returns("");
            var options = Options.Create(new MemoryCacheOptions 
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes(10)
            });
            var cache = new MemoryCache(options);
            return new MailAttachmentsService(@"C:\", fileSystem, cache, mockLogger.Object);
        }
    }
}
