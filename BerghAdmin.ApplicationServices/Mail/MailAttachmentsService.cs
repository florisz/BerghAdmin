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
            IMemoryCache cache,
            ILogger<MailAttachmentsService> logger)
        {
            _contentRoot = contentRoot;
            _fileSystem = fileSystem;
            _cache = cache;
            _logger = logger;
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

            var sb = new StringBuilder(message.HtmlBody);
            for (int count = 0; count < imageMatches.Count; count++)
            {
                var imageCapture = imageMatches[count];
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

                var cacheKey = imageSource.ToLowerInvariant();
                if (!_cache.TryGetValue(cacheKey, out MailAttachment attachment))
                {
                    var filePath = _fileSystem.Path.Combine(_contentRoot, imageSource);
                    if (!_fileSystem.File.Exists(filePath))
                    {
                        continue;
                    }

                    var fileName = _fileSystem.Path.GetFileName(cacheKey);
                    var contentType = GetContentType(fileName);
                    attachment = await _cache.GetOrCreateAsync(cacheKey, (entry) =>
                    {
                        var base64Content = Base64EncodeFile(filePath);
                        var contentId = Guid.NewGuid().ToString();
                        var attachment = new MailAttachment(fileName, contentType, base64Content, contentId);
                        _logger.LogInformation("Adding server image {serverImage} to cache for inlined attachments with key {cacheKey}",
                            imageSource,
                            cacheKey);
                        entry.SetValue(attachment);
                        return Task.FromResult(attachment);
                    });
                }

                _logger.LogInformation("Replacing server image {serverImage} with inlined attachment {fileNameOnServer} with id {contentId}",
                    imageSource,
                    attachment.FilenameOnServer,
                    attachment.ContentID);
                sb.Replace(imageSource, $"cid:{attachment.ContentID}");
                message.InlinedAttachments.Add(attachment);
            }

            message.HtmlBody = sb.ToString();
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
        private readonly IMemoryCache _cache;
        private readonly ILogger<MailAttachmentsService> _logger;
        private readonly Regex _imageRegex = new(
            @"<img[^>]+src=""([^"">]+)""",
            RegexOptions.Singleline | RegexOptions.IgnoreCase | RegexOptions.Compiled);
    }
}
