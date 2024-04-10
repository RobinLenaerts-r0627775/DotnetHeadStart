namespace DotnetHeadStart;

/// <summary>
/// Base Identity Context for all Identity contexts. Contains the BaseModel query filter.
/// </summary>
/// <typeparam name="TUser"></typeparam>
/// <param name="options"></param>
public class BaseIdentityContext<TUser>(DbContextOptions options) : IdentityDbContext<TUser>(options), IBaseContext where TUser : IdentityUser
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.AddBaseModelQueryFilter();
        base.OnModelCreating(modelBuilder);
    }
}
