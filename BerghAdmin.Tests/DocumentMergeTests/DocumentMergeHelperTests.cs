using System;
using System.IO;
using BerghAdmin.DbContexts;
using BerghAdmin.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using BerghAdmin.Services;

namespace BerghAdmin.DocumentMergeTests
{
    public static class DocumentMergeHelperTests
    {
        public static SqliteConnection GetSqliteInMemoryConnection()
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = ":memory:" };

            return new SqliteConnection(connectionStringBuilder.ToString());
        }

        public static DbContextOptions<ApplicationDbContext> GetApplicationDbContextOptions(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(connection)
                .Options;

            return options;
        }

        public static void CreateTestDataBaseInMemory(IServiceProvider serviceProvider, List<TestDocument> testDocuments)
        {
            // You cannot use 'using' here. Otherwise you are disposing the context that the serviceProvider holds for us
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.OpenConnection();
            dbContext.Database.EnsureCreated();
            CreateData(dbContext, testDocuments);
        }

        private static void CreateData(ApplicationDbContext dbContext, List<TestDocument> testDocuments)
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
                    ContentType = ContentTypeEnum.Word,
                    Content = content,
                    IsMergeTemplate = true
                });
            }
            dbContext.SaveChangesAsync();
        }
    }
}
