namespace DotnetHeadStart;

/// <summary>
/// Implementation of the DbContext class. Base functionalities: 
/// - Automatically set the CreatedAt, ModifiedAt and DeletedAt properties of the BaseModel entities.
/// </summary>
public class BaseContext(DbContextOptions options) : DbContext(options)
{

    /// <summary>
    /// Save the changes done to the database. Automatically sets the CreatedAt, ModifiedAt and DeletedAt properties of the BaseModel entities.
    /// This method is not the default EF Core SaveChanges method, but an extension of it.
    /// </summary>
    /// <param name="softdelete">If true, the DeletedAt property will be set (only when the entity is implements <seealso cref="BaseModel">BaseModel</seealso>) instead of deleting the entity. Default value is true</param>
    public int SaveChanges(bool softdelete = true)
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
                        if (softdelete)
                        {
                            entityEntry.State = EntityState.Modified;
                            model.DeletedAt = DateTime.Now;
                        }
                        break;
                }
            }
        }
        return base.SaveChanges();
    }
}