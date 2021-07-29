using NUnit.Framework;
using bihz.kantoorportaal.DbContexts;
using bihz.kantoorportaal.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using bihz.kantoorportaal.Data;
using System.IO;

namespace bihz_kantoorportaal.tests
{
    [TestFixture]
    public class MergeTemplateTests
    {
        private ServiceProvider _serviceProvider;
        private const string DocumentPath = "C:/git/bihz/bihz-kantoorportaal/bihz-kantoorportaal.tests/MergeTemplateTests/TestDocumenten"; 
        private List<TestDocument> TestDocuments = new List<TestDocument> {
            new TestDocument() 
            { 
                Id = 1, 
                Name = "TestTemplate1", 
                FilePath = $"{DocumentPath}/TemplateFactuurSponsor.docx"
            },
            new TestDocument() 
            { 
                Id = 2, 
                Name = "TestTemplate2", 
                FilePath = $"{DocumentPath}/TemplateFactuurSponsor.docx"
            },
        };
                    
        [SetUp]
        public void SetupMergeTemplateTests()
        {
            var connection = MergeTemplateHelperTests.GetSqliteInMemoryConnection();
            var options = MergeTemplateHelperTests.GetApplicationDbContextOptions(connection);
            MergeTemplateHelperTests.CreateTestDataBaseInMemory(options, TestDocuments);

            var services = new ServiceCollection();
            services
                .AddDbContext<ApplicationDbContext>(builder => 
                {
                    builder.UseSqlite(connection);
                })
                .AddEntityFrameworkSqlite()
                .AddScoped<IDocumentService, DocumentService>()
                .AddScoped<IMergeService, MergeService>();
            
            // Build the service provider
            _serviceProvider = services.BuildServiceProvider();
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
            var service = _serviceProvider.GetRequiredService<IMergeService>();
            var template = service.GetMergeTemplateById(2);
            Assert.NotNull(template);
            Assert.AreEqual(template.MergeDocument.Name, "TestTemplate2");
            var mergeFields = template.MergeFields;
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
        public void TestMergeTemplate2Document()
        {
            var mergeService = _serviceProvider.GetRequiredService<IMergeService>();
            var template = mergeService.GetMergeTemplateById(2);
            Assert.NotNull(template);
            Assert.AreEqual(template.MergeDocument.Name, "TestTemplate2");
            var mergeFields = template.MergeFields;
            var mergeDictionary = new Dictionary<string, string>();
            mergeDictionary["Bedrijfsnaam"] = "The Merge Company";
            mergeDictionary["NaamAanhef"] = "Mr. the Merge";
            mergeDictionary["StraatEnNummer"] = "Mergestreet 42";
            mergeDictionary["Plaatsnaam"] = "Mergecity";
            mergeDictionary["Postcode"] = "5555 XX";
            mergeDictionary["HuidigeDatum"] = "12-12-2021";
            mergeDictionary["SponsorBedrag"] = "42";

            var mergedStream = mergeService.Merge(template, mergeDictionary);
            Assert.IsTrue(mergedStream.Length > 0);
            using(FileStream outputFileStream = new FileStream("c:/temp/output.docx", FileMode.Create))
            { 
                mergedStream.Position = 0;
                mergedStream.CopyTo(outputFileStream);  
            }
        }

    }
}