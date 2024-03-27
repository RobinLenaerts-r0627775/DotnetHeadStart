using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace DotnetHeadStart;

public class BaseModelInterceptor : SaveChangesInterceptor
{

    /// <summary>
    /// Automatically set the CreatedAt, ModifiedAt and DeletedAt properties of the BaseModel entities.
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

        IEnumerable<EntityEntry<BaseModel>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<BaseModel>()
                .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added);

        foreach (EntityEntry<BaseModel> entry in entries)
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
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
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

        IEnumerable<EntityEntry<BaseModel>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<BaseModel>()
                .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added);

        foreach (EntityEntry<BaseModel> entry in entries)
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
                    entry.State = EntityState.Modified;
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    break;
            }
        }
        return base.SavingChanges(eventData, result);
    }
}
