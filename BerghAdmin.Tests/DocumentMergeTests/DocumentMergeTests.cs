using NUnit.Framework;
using BerghAdmin.DbContexts;
using BerghAdmin.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using BerghAdmin.Data;
using System.IO;

namespace BerghAdmin.DocumentMergeTests
{
    [TestFixture]
    public class DocumentMergeTests
    {
        private readonly ServiceProvider _serviceProvider;
        private const string DocumentPath = "C:/git/bihz/BerghAdmin/BerghAdmin.Tests/DocumentMergeTests/TestDocumenten"; 
        private readonly List<TestDocument> TestDocuments = new()
        {
            new TestDocument() 
            { 
                Id = 1, 
                Name = "TestTemplate1", 
                FilePath = $"./DocumentMergeTests/TestDocumenten/TemplateFactuurSponsor.docx"
            },
            new TestDocument() 
            { 
                Id = 2, 
                Name = "TestTemplate2", 
                FilePath = $"./DocumentMergeTests/TestDocumenten/TemplateFactuurSponsor.docx"
            },
            new TestDocument() 
            { 
                Id = 3, 
                Name = "TestTemplate3", 
                FilePath = $"./DocumentMergeTests/TestDocumenten/TestTemplate3.docx"
            },
        };

        public DocumentMergeTests()
        {
            var connection = DocumentMergeHelperTests.GetSqliteInMemoryConnection();
            var services = new ServiceCollection();
            services
                .AddEntityFrameworkSqlite()
                .AddDbContext<ApplicationDbContext>(builder =>
                {
                    builder.UseSqlite(connection);
                }, ServiceLifetime.Singleton, ServiceLifetime.Singleton)
                .AddScoped<IDocumentService, DocumentService>()
                .AddScoped<IPdfConverter, PdfConverter>()
                .AddScoped<IDocumentMergeService, DocumentMergeService>();

            _serviceProvider = services.BuildServiceProvider();
        
            DocumentMergeHelperTests.CreateTestDataBaseInMemory(_serviceProvider, TestDocuments);
        }

        [SetUp]
        public void SetupMergeTemplateTests()
        {
            //DocumentMergeHelperTests.CreateTestDataBaseInMemory(_serviceProvider, TestDocuments);
        }

        [Test]
        public void TestIsDocumentPresent()
        {
            var service = _serviceProvider.GetRequiredService<IDocumentService>();
            var doc = service.GetDocumentById(1);
            Assert.NotNull(doc);
            Assert.AreEqual(doc.Name, "TestTemplate1");
            Assert.Pass();
        }

        [Test]
        public void TestHasDocumentMergeFields()
        {
            var service = _serviceProvider.GetRequiredService<IDocumentMergeService>();
            var template = service.GetMergeTemplateById(2);
            Assert.NotNull(template);
            Assert.AreEqual(template!.Name, "TestTemplate2");
            var mergeFields = template!.GetMergeFields();
            Assert.Contains("Bedrijfsnaam", mergeFields);
            Assert.Contains("NaamAanhef", mergeFields);
            Assert.Contains("StraatEnNummer", mergeFields);
            Assert.Contains("Plaatsnaam", mergeFields);
            Assert.Contains("Postcode", mergeFields);
            Assert.Contains("HuidigeDatum", mergeFields);
            Assert.Contains("SponsorBedrag", mergeFields);
            Assert.Pass();
        }

        [Test]
        public void TestMergeTemplate3Document()
        {
            var mergeService = _serviceProvider.GetRequiredService<IDocumentMergeService>();
            var template = mergeService.GetMergeTemplateById(3);
            Assert.NotNull(template);
            Assert.AreEqual(template!.Name, "TestTemplate3");
            var mergeDictionary = new Dictionary<string, string>
            {
                ["MergeField1"] = "MergeField1.Value"
            };

            MergeDocument(mergeService, template, mergeDictionary);
        }


        [Test]
        public void TestMergeTemplate4Document()
        {
            var mergeService = _serviceProvider.GetRequiredService<IDocumentMergeService>();
            var template = mergeService.GetMergeTemplateById(2);

            Assert.NotNull(template);
            Assert.AreEqual(template!.Name, "TestTemplate2");

            var mergeDictionary = new Dictionary<string, string> 
            {
                { "Bedrijfsnaam", "Pieters en zn." },
                { "NaamAanhef", "de heer P. Pieters" },
                { "StraatEnNummer", "Pieterpad 12" },
                { "Postcode", "2961 XY" },
                { "Plaatsnaam", "Pieterbuuren" },
                { "HuidigeDatum", "24 juli 2021" },
                { "Sponsorbedrag", "€ 2.000,-" },
            };

            MergeDocument(mergeService, template, mergeDictionary);
        }

        private void MergeDocument(IDocumentMergeService mergeService, Document template, Dictionary<string, string> mergeDictionary)
        {
            var mergedStream = mergeService.Merge(template, mergeDictionary);
            Assert.IsTrue(mergedStream.Length > 0);
            using FileStream outputFileStream = new($"c:/temp/{template.Name}.docx", FileMode.Create);
            mergedStream.Position = 0;
            mergedStream.CopyTo(outputFileStream);
            var pdfService = _serviceProvider.GetRequiredService<IPdfConverter>();

            outputFileStream.Position = 0;
            var pdfStream = pdfService.ConvertWordToPdf(outputFileStream);
            var pdfOutputStream = new FileStream($"c:/temp/{template.Name}.pdf", FileMode.Create);

            pdfStream.Position = 0;
            pdfStream.CopyTo(pdfOutputStream);
            pdfStream.Close();
            pdfOutputStream.Close();
        }
    }
}