namespace DotnetHeadStart;

/// <summary>
/// Implementation of the DbContext class.
/// </summary>
public class BaseContext(DbContextOptions options) : DbContext(options), IBaseContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddBaseModelQueryFilter();
    }
}