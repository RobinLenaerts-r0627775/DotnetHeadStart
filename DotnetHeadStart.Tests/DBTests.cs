using Microsoft.EntityFrameworkCore;

namespace DotnetHeadStart.Tests;

public class DBTests
{
    private readonly TestContext _context;
    public DBTests()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<BaseContext>()
            .UseInMemoryDatabase(databaseName: "DBTests")
            .Options;
        _context = new TestContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
    }

    [Fact]
    public void CreatedAtIsSet()
    {
        // Arrange
        var test = new TestBaseModel { Name = "test" };

        // Act
        _context.Tests.Add(test);
        _context.SaveChanges();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.CreatedAt);
    }

    [Fact]
    public void ModifiedAtIsSet()
    {
        // Arrange
        var test = new TestBaseModel { Name = "test" };
        _context.Tests.Add(test);
        _context.SaveChanges();

        // Act
        test.Name = "test2";
        _context.SaveChanges();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.ModifiedAt);
    }

    [Fact]
    public void SoftDeleteChangesDeletedAtValue()
    {

        var test = new TestBaseModel { Name = "test" };
        _context.Tests.Add(test);
        _context.SaveChanges();

        // Act
        _context.Tests.Remove(test);
        _context.SaveChanges();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.DeletedAt);
    }
}

public class TestBaseModel : BaseModel
{
    public string Name { get; set; } = null!;
}

public class TestContext(DbContextOptions options) : BaseContext(options)
{
    public DbSet<TestBaseModel> Tests { get; set; }
}

