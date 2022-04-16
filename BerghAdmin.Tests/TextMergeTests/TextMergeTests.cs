using NUnit.Framework;
using System.Collections.Generic;
using BerghAdmin.Services.TextMerge;
using System.Linq;
using Moq;
using Microsoft.Extensions.Logging;

namespace BerghAdmin.Tests.MailMergeTests
{
    [TestFixture]
    public class MailMergeTests
    {
        // most simple valid text
        private const string _simpleText = "<p>small text</p>";
        // valid html, with some html attributes
        private const string _nonSimpleText =
            "<p><strong>Opening here</strong></p>" +
            "<p><strong>bold text</strong>​<br></p>" +
            "<p><em>​Italic</em>​<br></p>" +
            "<p><strong>​<em>​<span style=\"text-decoration: underline;\">​Underlined italic bold text</span></em></strong>​<br></p>" +
            "<blockquote>quoted text</blockquote>" +
            "<pre>source code<br></pre>";
        // valid html with one merge field
        private const string _textWithOpeningsTagOnly = "<p>Hallo &lt;&lt;naam</p>";
        private const string _textWithClosingTagOnly = "<p>Hallo naam&gt;&gt;</p>";
        private const string _textWithInvalidMergeField1 = "<p>Hallo &lt;&lt;aap&gt;&gt;</p>";
        private const string _textWithInvalidMergeField2 = "&lt;&lt;aap&gt;&gt;";
        private const string _textWithOneMergeField = "<p>Hallo &lt;&lt;naam&gt;&gt;</p>";
        private const string _textWithOneMergeFieldTwice = "<p>Hallo &lt;&lt;naam&gt;&gt;&lt;&lt;naam&gt;&gt;</p>";
        private const string _textWithMultipleMergeFields = "<p>Hallo &lt;&lt;naam&gt;&gt;<br/>wonend op: &lt;&lt;adres&gt;&gt;<br/>te: &lt;&lt;postcode&gt;&gt; &lt;&lt;woonplaats&gt;&gt;</p>";

        private readonly Dictionary<string, string> _mergeFieldValues = new()
        {
            { "naam", "Jan Jansen" },
            { "adres", "Hoofdstraat 1" },
            { "postcode", "1234 AB" },
            { "woonplaats", "Amsterdam" }
        };

        [SetUp]
        public void SetupTextMergeTests()
        {

        }

        [Test]
        public void TestTextIsEmpty()
        {
            var mergeResult = CreateService().Merge((string?)null, null);
            Assert.AreEqual("", mergeResult);
        }

        [Test]
        public void TestSimpleTextHasNoMergeFieldsWithoutDictionary()
        {
            var mergeResult = CreateService().Merge(_simpleText, null);
            Assert.AreEqual(_simpleText, mergeResult);
        }

        [Test]
        public void TestSimpleTextHasNoMergeFieldsWithDictionary()
        {
            var mergeResult = CreateService().Merge(_simpleText, _mergeFieldValues);
            Assert.AreEqual(_simpleText, mergeResult);
        }

        [Test]
        public void TestNonSimpleTextHasNoMergeFieldsWithoutDictionary()
        {
            var mergeResult = CreateService().Merge(_nonSimpleText, null);
            Assert.AreEqual(_nonSimpleText, mergeResult);
        }

        [Test]
        public void TestNonSimpleTextHasNoMergeFieldsWithDictionary()
        {
            var mergeResult = CreateService().Merge(_nonSimpleText, _mergeFieldValues);
            Assert.AreEqual(_nonSimpleText, mergeResult);
        }

        [Test]
        public void TestWithOpeningsTagOnly()
        {
            Assert.Throws<TextMergeNoClosingTagException>(() => CreateService().Merge(_textWithOpeningsTagOnly, _mergeFieldValues));
        }

        [Test]
        public void TestWithClosingTagOnly()
        {
            Assert.Throws<TextMergeNoOpeningTagException>(() => CreateService().Merge(_textWithClosingTagOnly, _mergeFieldValues));
        }

        [Test]
        public void TestMergeFieldNoMergeValues()
        {
            Assert.Throws<TextMergeNoMergeFieldException>(() => CreateService().Merge(_textWithInvalidMergeField1, null));
        }

        [Test]
        public void TestMergeFieldUnknown()
        {
            Assert.Throws<TextMergeNoMergeFieldException>(() => CreateService().Merge(_textWithInvalidMergeField1, _mergeFieldValues));
        }

        [Test]
        public void TestMergeFieldInvalidInHtml()
        {
            Assert.Throws<TextMergeNoMergeFieldException>(() => CreateService().Merge(_textWithInvalidMergeField2, null));
        }

        [Test]
        public void TestHasOneMergeFieldWithDictionary()
        {
            var mergeResult = CreateService().Merge(_textWithOneMergeField, _mergeFieldValues);
            Assert.AreEqual("<p>Hallo Jan Jansen</p>", mergeResult);
        }

        [Test]
        public void TestHasOneMergeFieldTwiceWithDictionary()
        {
            var mergeResult = CreateService().Merge(_textWithOneMergeFieldTwice, _mergeFieldValues);
            Assert.AreEqual("<p>Hallo Jan JansenJan Jansen</p>", mergeResult);
        }

        [Test]
        public void TestHasMultipleMergeFieldWithDictionary()
        {
            var mergeResult = CreateService().Merge(_textWithMultipleMergeFields, _mergeFieldValues);
            Assert.AreEqual("<p>Hallo Jan Jansen<br/>wonend op: Hoofdstraat 1<br/>te: 1234 AB Amsterdam</p>", mergeResult);
        }
        private static TextMergeService CreateService()
        {
            return new TextMergeService(Mock.Of<ILogger<TextMergeService>>());
        }
    }
}
