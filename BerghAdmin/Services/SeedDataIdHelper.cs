using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.Services
{
    public static class SeedDataIdHelper
    {
        public static Task EnableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: true);
        public static Task DisableIdentityInsert<T>(this DbContext context) => SetIdentityInsert<T>(context, enable: false);

        private static Task SetIdentityInsert<T>(DbContext context, bool enable)
        {
            var entityType = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";
            var sqlStmt = $"SET IDENTITY_INSERT dbo.{entityType.GetTableName()} {value}";
            return context.Database.ExecuteSqlRawAsync(sqlStmt);
        }

        public static async void SaveChangesWithoutIdentityInsert<T>(this DbContext context)
        {
            var strategy = context.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    await context.EnableIdentityInsert<T>();
                    await context.SaveChangesAsync();
                    await context.DisableIdentityInsert<T>();

                    transaction.Commit();
                }
            });
        }    
    }
}
