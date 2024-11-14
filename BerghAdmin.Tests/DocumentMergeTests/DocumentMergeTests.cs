using NUnit.Framework;

using BerghAdmin.DbContexts;
using BerghAdmin.Data;
using BerghAdmin.Tests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using BerghAdmin.Services.Documenten;

namespace BerghAdmin.DocumentMergeTests;

[TestFixture]
public class DocumentMergeTests : DatabaseTestSetup
{
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
        new TestDocument()
        {
            Id = 4,
            Name = "TestTemplate4",
            FilePath = $"./DocumentMergeTests/TestDocumenten/AmbassadeurFactuurTemplate.docx"
        }
    };

    protected override void RegisterServices(ServiceCollection services)
    {
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IPdfConverter, PdfConverter>();
        services.AddScoped<IDocumentMergeService, DocumentMergeService>();
        services.AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
    }

    [SetUp]
    public async Task SetupData()
    {
        if (this.ApplicationDbContext == null)
            throw new ArgumentNullException("ApplicationDbContext");

        await CreateDataAsync(this.ApplicationDbContext, TestDocuments);
    }

    [Test]
    public void TestIsDocumentPresent()
    {
        var service = this.GetRequiredService<IDocumentService>();
        var doc = service.GetDocumentById(1);
        Assert.NotNull(doc);
        
        // not really necessary but to avoid warnings
        if (doc != null)
        {
            Assert.AreEqual(doc.Name, "TestTemplate1");
        }
    }

    [Test]
    public void TestHasDocumentMergeFields()
    {
        var service = this.GetRequiredService<IDocumentMergeService>();
        var template = service.GetMergeTemplateById(4);
        Assert.NotNull(template);
        Assert.AreEqual(template!.Name, "TestTemplate4");
        var mergeFields = service.GetMergeFieldsFor(template).ToArray();
        Assert.Contains("AmbassadeurNaam", mergeFields);
        Assert.Contains("ContactpersoonAanhef", mergeFields);
        Assert.Contains("AmbassadeurAdres", mergeFields);
        Assert.Contains("AmbassadeurWoonplaats", mergeFields);
        Assert.Contains("AmbassadeurPostcode", mergeFields);
        Assert.Contains("Dagtekening", mergeFields);
        Assert.Contains("FactuurBedrag", mergeFields);
        Assert.Pass();
    }

    [Test]
    public void TestMergeTemplate2Document()
    {
        var mergeService = this.GetRequiredService<IDocumentMergeService>();
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

    [Test]
    public void TestMergeTemplate3Document()
    {
        var mergeService = this.GetRequiredService<IDocumentMergeService>();
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
    public void TestMergeAmbassadeurFactuurTemplateDocument()
    {
        var mergeService = this.GetRequiredService<IDocumentMergeService>();
        var template = mergeService.GetMergeTemplateById(4);

        Assert.NotNull(template);
        Assert.AreEqual(template!.Name, "TestTemplate4");

        var mergeDictionary = new Dictionary<string, string>
        {
            { "AmbassadeurNaam", "Royal Fassin B.V." },
            { "ContactpersoonAanhef", "de heer" },
            { "ContactpersoonVoornaam", "Thomas" },
            { "ContactpersoonAchternaam", "Fassin" },
            { "AmbassadeurAdres", "Ulenpasweg 8" },
            { "AmbassadeurPostcode", "7041 GB" },
            { "AmbassadeurWoonplaats", "'s Heerenberg" },
            { "Dagtekening", "25 oktober 2023" },
            { "FactuurNummer", "202300752" },
            { "DebiteurNummer", "00172" },
            { "HuidigeDatum", "24 juli 2021" },
            { "FactuurBedrag", "€ 5.000,-" },
            { "FactuurTotaalBedrag", "€ 5.000,-" },
        };

        MergeDocument(mergeService, template, mergeDictionary);
    }

    private void MergeDocument(IDocumentMergeService mergeService, Document template, Dictionary<string, string> mergeDictionary)
    {
        using var templateStream = new MemoryStream(template.Content);
        var mergedStream = mergeService.Merge(templateStream, mergeDictionary);
        Assert.IsTrue(mergedStream.Length > 0);
        using FileStream outputFileStream = new($"./DocumentMergeTests/TestDocumenten/{template.Name}.docx", FileMode.Create);
        mergedStream.Position = 0;
        mergedStream.CopyTo(outputFileStream);
        var pdfService = this.GetRequiredService<IPdfConverter>();

        outputFileStream.Position = 0;
        var pdfStream = pdfService.ConvertWordToPdf(outputFileStream);
        var pdfOutputStream = new FileStream($"./DocumentMergeTests/TestDocumenten/{template.Name}.pdf", FileMode.Create);

        pdfStream.Position = 0;
        pdfStream.CopyTo(pdfOutputStream);
        pdfStream.Close();
        pdfOutputStream.Close();
    }

    private static async Task CreateDataAsync(ApplicationDbContext dbContext, List<TestDocument> testDocuments)
    {
        foreach (var testDocument in testDocuments)
        {
            if (!File.Exists(testDocument.FilePath))
            {
                throw new ApplicationException($"Test document with path {testDocument.FilePath} does not exist");
            }
            var content = File.ReadAllBytes(testDocument.FilePath);
            dbContext.Documenten?.Add(new Document
            {
                Id = testDocument.Id,
                Name = testDocument.Name,
                DocumentType = DocumentTypeEnum.Word,
                Content = content,
                IsMergeTemplate = true,
                Owner = "Test"
            });
        }
        await dbContext.SaveChangesAsync();
    }

}