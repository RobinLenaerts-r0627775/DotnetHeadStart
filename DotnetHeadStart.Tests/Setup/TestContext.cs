using DotnetHeadStart.DB;

namespace DotnetHeadStart.Tests;

public class TestContext(DbContextOptions<TestContext> options) : BaseContext(options)
{
    public DbSet<TestObject> TestObjects { get; set; } = null!;

}
