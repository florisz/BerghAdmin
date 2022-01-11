using System;
using System.IO;
using BerghAdmin.DbContexts;
using BerghAdmin.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using BerghAdmin.Services;

namespace BerghAdmin.DocumentMergeTests
{
    public static class DocumentMergeTestHelper
    {
        public static void CreateTestDataInMemoryDatabase(IDocumentService documentService, List<TestDocument> testDocuments)
        {
            foreach (var testDocument in testDocuments)
            {
                if (! File.Exists(testDocument.FilePath))
                {
                    throw new ApplicationException($"Test document with path {testDocument.FilePath} does not exist");
                }
                var content = File.ReadAllBytes (testDocument.FilePath);
                documentService.SaveDocument(new Document 
                                            {
                                                Id = testDocument.Id,
                                                Name = testDocument.Name,
                                                ContentType = ContentTypeEnum.Word,
                                                Content = content,
                                                IsMergeTemplate = true
                                            });
            }
        }
    }
}
