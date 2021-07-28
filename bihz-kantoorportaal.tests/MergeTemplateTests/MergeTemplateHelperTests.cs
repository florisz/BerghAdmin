using System;
using System.IO;
using bihz.kantoorportaal.DbContexts;
using bihz.kantoorportaal.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace bihz_kantoorportaal.tests
{
    public static class MergeTemplateHelperTests
    {
        public static SqliteConnection GetSqliteInMemoryConnection()
        {
            var connectionStringBuilder =
                new SqliteConnectionStringBuilder { DataSource = ":memory:" };
            
            return new SqliteConnection(connectionStringBuilder.ToString());
        }

        public static DbContextOptions<ApplicationDbContext> GetApplicationDbContextOptions(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            return options;
        }

        public static async void CreateTestDataBaseInMemory(DbContextOptions<ApplicationDbContext> options, List<TestDocument> testDocuments)
        {
            await using (var context = new ApplicationDbContext(options))
            {
                await context.Database.OpenConnectionAsync();
                await context.Database.EnsureCreatedAsync();
                CreateData(context, testDocuments);
            }
        }

        private static void CreateData(ApplicationDbContext applicationDbContext, List<TestDocument> testDocuments)
        {
            foreach (var testDocument in testDocuments)
            {
                if (! File.Exists(testDocument.FilePath))
                {
                    throw new ApplicationException($"Test document with path {testDocument.FilePath} does not exist");
                }
                var content = File.ReadAllBytes (testDocument.FilePath);
                applicationDbContext.Documenten.Add(new Document 
                                                            {
                                                                Id = testDocument.Id,
                                                                Name = testDocument.Name,
                                                                ContentType = ContentTypeEnum.Word,
                                                                Content = content,
                                                                IsMergeTemplate = true
                                                            });
            }
            applicationDbContext.SaveChangesAsync();
        }
    }
}
