namespace DotnetHeadStart.Test;

public class BaseModelTests
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
    private void Dispose()
    {
        _context.Dispose();
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

        // act
        _context.TestObjects.Add(testObject);
        _context.SaveChanges();

        // assert
        Assert.Equal(1, _context.TestObjects.Count());
        Assert.Equal("Test Object", _context.TestObjects.First().Name);
        Assert.NotEqual(testObject.CreatedAt, DateTime.MinValue);
    }
}

//dispose

public class TestObject : BaseModel
{
    public string Name { get; set; }
    public string Description { get; set; }
}

public class TestDbContext : HeadStartContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options) : base(options)
    {
    }
    public DbSet<TestObject> TestObjects { get; set; }
}