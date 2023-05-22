namespace DotnetHeadStart.DB;

public class HeadStartContext : DbContext
{
    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
        foreach (var entityEntry in entries)
        {
            if(entityEntry.Entity is BaseModel model)
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
                        model.DeletedAt = DateTime.Now;
                        entityEntry.State = EntityState.Modified;
                        break;
                }
            }
        }
        return base.SaveChanges();
    }
}