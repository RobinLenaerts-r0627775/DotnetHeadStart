
namespace DotnetHeadStart.DB;

/// <summary>
/// Implementation of the DbContext class. Base functionalities: 
/// - Automatically set the CreatedAt, ModifiedAt and DeletedAt properties of the BaseModel entities.
/// </summary>
public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions options) : base(options)
    {
    }

    /// <summary>
    /// Save the changes done to the database. Automatically sets the CreatedAt, ModifiedAt and DeletedAt properties of the BaseModel entities.
    /// </summary>
    /// <param name="hardDelete">Boolean indicating whether or not the entities should be hard deleted or not. Only applies if the entity is of basetype BaseModel</param>
    public new int SaveChanges(bool hardDelete = false)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
        foreach (var entityEntry in entries)
        {
            if (entityEntry.Entity is BaseModel model)
            {
                switch (entityEntry.State)
                {
                    case EntityState.Added:
                        model.CreatedAt = DateTime.Now;
                        break;
                    case EntityState.Modified:
                        model.ModifiedAt = DateTime.Now;
                        break;
                    case EntityState.Deleted:
                        if (!hardDelete)
                        {
                            model.DeletedAt = DateTime.Now;
                            entityEntry.State = EntityState.Modified;
                        }
                        break;
                }
            }
        }
        return base.SaveChanges();
    }
}