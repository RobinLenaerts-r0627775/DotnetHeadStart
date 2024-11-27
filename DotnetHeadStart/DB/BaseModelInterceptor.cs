using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DotnetHeadStart.DB;

public class BaseModelInterceptor : SaveChangesInterceptor
{

    /// <summary>
    /// Automatically set the CreatedAt, ModifiedAt and DeletedAt properties of the IBaseEntity entities.
    /// </summary>
    /// <param name="eventData"></param>
    /// <param name="result"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(
                eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<IBaseEntity>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<IBaseEntity>()
                .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added);

        foreach (EntityEntry<IBaseEntity> entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    if (entry is ISoftDeletable softDeletable)
                    {
                        entry.State = EntityState.Modified;
                        softDeletable.DeletedAt = DateTime.UtcNow;
                    }
                    break;
            }
        }
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    //Same for SaveChanges
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context is null)
        {
            return base.SavingChanges(
                eventData, result);
        }

        IEnumerable<EntityEntry<IBaseEntity>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<IBaseEntity>()
                .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added);

        foreach (EntityEntry<IBaseEntity> entry in entries)
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.ModifiedAt = DateTime.UtcNow;
                    break;
                case EntityState.Deleted:
                    if (entry is ISoftDeletable softDeletable)
                    {
                        entry.State = EntityState.Modified;
                        softDeletable.DeletedAt = DateTime.UtcNow;
                    }
                    break;
            }
        }
        return base.SavingChanges(eventData, result);
    }
}
