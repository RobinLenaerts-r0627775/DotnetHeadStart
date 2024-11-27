namespace DotnetHeadStart.Tests;

public class DBSoftDeletableTests : IDisposable
{
    private readonly TestSoftDeletableRepository _repo;
    private readonly TestContext _context;
    public DBSoftDeletableTests()
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

        _repo = new TestSoftDeletableRepository(_context);

        //Seed the database
        _repo.CreateAsync(new TestObjectSoftDeletable { Name = "test" }).Wait();
        _repo.CreateAsync(new TestObjectSoftDeletable { Name = "test2" }).Wait();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public void SoftDeleteChangesDeletedAtValue()
    {

        var test = new TestObjectSoftDeletable { Name = "test" };
        _context.TestObjectsSoftDeletable.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjectsSoftDeletable.Remove(test);
        _context.SaveChanges();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.DeletedAt);
    }

    [Fact]
    public void SoftDeleteDoesNotDelete()
    {
        // Arrange
        var test = new TestObjectSoftDeletable { Name = "test" };
        _context.TestObjectsSoftDeletable.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjectsSoftDeletable.Remove(test);
        _context.SaveChanges();

        // Assert
        Assert.NotNull(_context.TestObjectsSoftDeletable.IgnoreQueryFilters().FirstOrDefault(t => t.Id == test.Id));
    }

    [Fact]
    public void GetByIdReturnsDeletedObjectIfDeleted()
    {
        // Arrange
        var test = new TestObjectSoftDeletable { Name = "test" };
        _context.TestObjectsSoftDeletable.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjectsSoftDeletable.Remove(test);
        _context.SaveChanges();
        var result = _repo.GetById(test.Id, true);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task SoftDeleteChangesDeletedAtValueAsync()
    {

        var test = new TestObjectSoftDeletable { Name = "test" };
        _context.TestObjectsSoftDeletable.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjectsSoftDeletable.Remove(test);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.DeletedAt);
    }

    [Fact]
    public async Task SoftDeleteDoesNotDeleteAsync()
    {
        // Arrange
        var test = new TestObjectSoftDeletable { Name = "test" };
        _context.TestObjectsSoftDeletable.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjectsSoftDeletable.Remove(test);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(_context.TestObjectsSoftDeletable.Find(test.Id));
    }

    [Fact]
    public async Task GetAllReturnsAllObjectsWithoutDeletedAsync()
    {
        // Arrange
        var test = new TestObjectSoftDeletable { Name = "test" };
        _context.TestObjectsSoftDeletable.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjectsSoftDeletable.Remove(test);
        await _context.SaveChangesAsync();
        var result = await _repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdReturnsDeletedObjectIfDeletedAsync()
    {
        // Arrange
        var test = new TestObjectSoftDeletable { Name = "test" };
        _context.TestObjectsSoftDeletable.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjectsSoftDeletable.Remove(test);
        await _context.SaveChangesAsync();
        var result = await _repo.GetByIdAsync(test.Id, true);

        // Assert
        Assert.NotNull(result);
    }





}
