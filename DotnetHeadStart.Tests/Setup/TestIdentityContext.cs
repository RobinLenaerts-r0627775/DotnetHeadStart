namespace DotnetHeadStart.Tests;

public class TestIdentityContext(DbContextOptions options) : BaseIdentityContext<TestUser>(options)
{
    public DbSet<TestObject> TestObjects { get; set; } = null!;
}
