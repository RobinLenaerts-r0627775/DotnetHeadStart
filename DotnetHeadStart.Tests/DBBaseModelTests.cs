namespace DotnetHeadStart.Tests;

public class DBBaseModelTests : IDisposable
{
    private readonly TestRepository _repo;
    private readonly TestContext _context;
    public DBBaseModelTests()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<TestContext>()
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning))
            .UseInMemoryDatabase(databaseName: "DBTests", new InMemoryDatabaseRoot())
            .AddInterceptors(new BaseModelInterceptor())
            .Options;
        _context = new TestContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repo = new TestRepository(_context);

        //Seed the database
        _repo.CreateAsync(new TestObject { Name = "test" }).Wait();
        _repo.CreateAsync(new TestObject { Name = "test2" }).Wait();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    #region SyncTests

    [Fact]
    public void CreatedAtIsSet()
    {
        // Arrange
        var test = new TestObject { Name = "testnew" };

        // Act
        _repo.Create(test);

        var newlyCreated = _repo.GetById(test.Id);

        // Assert
        Assert.NotEqual(DateTime.MinValue, newlyCreated?.CreatedAt);
    }

    [Fact]
    public void ModifiedAtIsSet()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        test.Name = "test2";
        _context.SaveChanges();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.ModifiedAt);
    }

    [Fact]
    public void GetByIdReturnsCorrectObject()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        var result = _repo.GetById(test.Id);

        // Assert
        Assert.Equal(test, result);
    }

    [Fact]
    public void GetByIdReturnsNullIfNotFound()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        var result = _repo.GetById(test.Id + 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetByIdReturnsNullIfDeleted()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        _context.SaveChanges();
        var result = _repo.GetById(test.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetAllReturnsAllObjects()
    {
        // Act
        var result = _repo.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public void GetAllReturnsAllObjectsWithoutDeleted()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        _context.SaveChanges();
        var result = _repo.GetAll();

        // Assert
        Assert.Equal(2, result.Count());
    }
    #endregion

    #region AsyncTests

    [Fact]
    public async Task CreatedAtIsSetAsync()
    {
        // Arrange
        var test = new TestObject { Name = "testnew" };

        // Act
        await _repo.CreateAsync(test);

        var newlyCreated = await _repo.GetByIdAsync(test.Id);

        // Assert
        Assert.NotEqual(DateTime.MinValue, newlyCreated?.CreatedAt);
    }

    [Fact]
    public async Task ModifiedAtIsSetAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        test.Name = "test2";
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.ModifiedAt);
    }

    [Fact]
    public async Task GetByIdReturnsCorrectObjectAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        var result = await _repo.GetByIdAsync(test.Id);

        // Assert
        Assert.Equal(test, result);
    }

    [Fact]
    public async Task GetByIdReturnsNullIfNotFoundAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        var result = await _repo.GetByIdAsync(test.Id + 1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdReturnsNullIfDeletedAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        await _context.SaveChangesAsync();
        var result = await _repo.GetByIdAsync(test.Id);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetAllReturnsAllObjectsAsync()
    {
        // Act
        var result = await _repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    #endregion

}
