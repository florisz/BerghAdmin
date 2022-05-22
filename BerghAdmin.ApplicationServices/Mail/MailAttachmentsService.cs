using System.IO.Abstractions;
using System.Text;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace BerghAdmin.ApplicationServices.Mail
{
    public class MailAttachmentsService : IMailAttachmentsService
    {
        public MailAttachmentsService(
            string contentRoot,
            IFileSystem fileSystem,
            ILogger<MailAttachmentsService> logger)
        {
            _contentRoot = contentRoot;
            _fileSystem = fileSystem;
            this.logger = logger;
        }

        public async Task ReplaceServerImagesWithInlinedAttachmentsAsync(MailMessage message)
        {
            if (message.HtmlBody == null)
            {
                return;
            }

            var imageMatches = _imageRegex.Matches(message.HtmlBody);
            if (imageMatches == null)
            {
                return;
            }

            foreach (Match imageCapture in imageMatches)
            {
                if (imageCapture.Groups.Count < 2)
                {
                    continue;
                }

                var imageSource = imageCapture.Groups[1].Value;
                if (string.IsNullOrWhiteSpace(imageSource))
                {
                    continue;
                }

                if (imageSource.StartsWith("http://") || imageSource.StartsWith("https://"))
                {
                    continue;
                }

                var filePath = _fileSystem.Path.Combine(_contentRoot, imageSource);
                if (!_fileSystem.File.Exists(filePath))
                {
                    continue;
                }

                var fileName = _fileSystem.Path.GetFileName(imageSource.ToLowerInvariant());
                var contentType = GetContentType(fileName);
                var base64Content = Base64EncodeFile(filePath);
                var contentId = Guid.NewGuid().ToString();
                var attachment = new MailAttachment(fileName, contentType, base64Content, contentId);

                logger.LogInformation("Replacing server image {serverImage} with inlined attachment {fileNameOnServer} with id {contentId}",
                    imageSource,
                    attachment.FilenameOnServer,
                    attachment.ContentID);

                message.HtmlBody = message.HtmlBody.Replace(imageSource, $"cid:{attachment.ContentID}");
                message.InlinedAttachments.Add(attachment);
            }
        }

        private string GetContentType(string fileName)
        {
            var extension = _fileSystem.Path.GetExtension(fileName).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                _ => $"image/{extension}"
            };
        }

        private string Base64EncodeFile(string filePath)
        {
            var logoBytes = _fileSystem.File.ReadAllBytes(filePath);
            var encoded = Convert.ToBase64String(logoBytes);
            return encoded;
        }

        private readonly string _contentRoot;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger<MailAttachmentsService> logger;
        private readonly Regex _imageRegex = new(
            @"<img[^>]+src=""([^"">]+)""",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
