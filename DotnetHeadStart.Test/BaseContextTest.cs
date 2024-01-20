namespace DotnetHeadStart.Test;

public class BaseModelTests : IDisposable
{
    protected readonly TestDbContext _context;
    public BaseModelTests()
    {
        // setup in memory database
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(databaseName: "HeadStart")
            .Options;
        _context = new TestDbContext(options);
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void CreatingModel_FillsInCreatedAt()
    {
        // arrange
        var testObject = new TestObject
        {
            Name = "Test Object",
            Description = "This is a test object"
        };

        var initialCount = _context.TestObjects.Count();

        _context.TestObjects.Add(testObject);
        _context.SaveChanges();

        // assert
        Assert.Equal(initialCount + 1, _context.TestObjects.Count());
        Assert.Equal("Test Object", _context.TestObjects.First().Name);
        Assert.NotEqual(testObject.CreatedAt, DateTime.MinValue);
    }

    [Fact]
    public void UpdatingModel_FillsInModifiedAt()
    {
        // arrange
        var testObject = new TestObject
        {
            Name = "Test Object",
            Description = "This is a test object"
        };
        _context.TestObjects.Add(testObject);
        _context.SaveChanges();

        var initialCount = _context.TestObjects.Count();

        // act
        testObject.Name = "Updated Test Object";
        _context.SaveChanges();

        // assert
        Assert.Equal(initialCount, _context.TestObjects.Count());
        Assert.Equal("Updated Test Object", _context.TestObjects.First().Name);
        Assert.NotEqual(testObject.ModifiedAt, DateTime.MinValue);
    }

    [Fact]
    public void DeletingModel_FillsInDeletedAt_AndDoesNotHardDelete()
    {
        // arrange
        var testObject = new TestObject
        {
            Name = "Test Object",
            Description = "This is a test object"
        };
        _context.TestObjects.Add(testObject);
        _context.SaveChanges();

        var initialCount = _context.TestObjects.Count();

        // act
        _context.TestObjects.Remove(testObject);
        _context.SaveChanges();

        // assert
        Assert.Equal(initialCount, _context.TestObjects.Count());
        Assert.NotEqual(testObject.DeletedAt, DateTime.MinValue);
    }

    [Fact]
    public void DeletingModel_HardDeletes_WhenHardDeleteIsTrue()
    {
        // arrange
        var testObject = new TestObject
        {
            Name = "Test Object",
            Description = "This is a test object"
        };
        _context.TestObjects.Add(testObject);
        _context.SaveChanges();

        var initialCount = _context.TestObjects.Count();

        // act
        _context.TestObjects.Remove(testObject);
        _context.SaveChanges(true);

        // assert
        Assert.Equal(initialCount - 1, _context.TestObjects.Count());
    }
}

//dispose

public class TestObject : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class TestDbContext : BaseContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
    public DbSet<TestObject> TestObjects { get; set; }
}