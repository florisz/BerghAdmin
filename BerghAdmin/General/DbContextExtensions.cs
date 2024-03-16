using Microsoft.EntityFrameworkCore;

namespace BerghAdmin.General;

public static class DbContextExtensions
{
    public static void UndoingChangesDbContextLevel(this DbContext context)
    {
        foreach (var entity in context.ChangeTracker.Entries())
        {
            switch (entity.State)
            {
                case EntityState.Modified:
                    entity.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entity.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entity.Reload();
                    break;
                default: break;
            }
        }
    }
}
