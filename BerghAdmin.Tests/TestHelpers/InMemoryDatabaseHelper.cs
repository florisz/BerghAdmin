
using BerghAdmin.DbContexts;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Tests.TestHelpers;
public static class InMemoryDatabaseHelper
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
}
