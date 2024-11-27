namespace DotnetHeadStart.Tests;

public class TestContext(DbContextOptions<TestContext> options) : BaseContext(options)
{
    public DbSet<TestObject> TestObjects { get; set; } = null!;
    public DbSet<TestObjectSoftDeletable> TestObjectsSoftDeletable { get; set; } = null!;

}
