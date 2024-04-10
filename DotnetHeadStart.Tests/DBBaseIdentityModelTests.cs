namespace DotnetHeadStart.Tests.Identity;

public class DBBaseIdentityModelTests : IDisposable, IClassFixture<IdentityFixture>
{
    private readonly TestIdentityContext _context;
    private readonly UserManager<TestUser> _userManager;
    private readonly TestRepository _repo;

    public DBBaseIdentityModelTests(IdentityFixture fixture)
    {
        var options = new DbContextOptionsBuilder<TestIdentityContext>()
            .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ManyServiceProvidersCreatedWarning))
            .UseInMemoryDatabase(databaseName: "DBTests", new InMemoryDatabaseRoot())
            .AddInterceptors(new BaseModelInterceptor())
            .Options;
        _context = new TestIdentityContext(options);

        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        _repo = new TestRepository(_context);
        _userManager = fixture.UserManager;

        //Seed the database
        _repo.CreateAsync(new TestObject { Name = "test" }).Wait();
        _repo.CreateAsync(new TestObject { Name = "test2" }).Wait();

        _userManager.CreateAsync(new TestUser { UserName = "testuser" }, "password").Wait();
        _userManager.CreateAsync(new TestUser { UserName = "testuser2" }, "password").Wait();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
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
    public void SoftDeleteChangesDeletedAtValue()
    {

        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        _context.SaveChanges();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.DeletedAt);
    }

    [Fact]
    public void SoftDeleteDoesNotDelete()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        _context.SaveChanges();

        // Assert
        Assert.NotNull(_context.TestObjects.IgnoreQueryFilters().FirstOrDefault(t => t.Id == test.Id));
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

    [Fact]
    public void GetAllReturnsAllObjectsWithDeleted()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        _context.SaveChanges();
        var result = _repo.GetAll(true);

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public void GetByIdReturnsDeletedObjectIfDeleted()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        _context.SaveChanges();
        var result = _repo.GetById(test.Id, true);

        // Assert
        Assert.NotNull(result);
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
    public async Task SoftDeleteChangesDeletedAtValueAsync()
    {

        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotEqual(DateTime.MinValue, test.DeletedAt);
    }

    [Fact]
    public async Task SoftDeleteDoesNotDeleteAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(_context.TestObjects.Find(test.Id));
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

    [Fact]
    public async Task GetAllReturnsAllObjectsWithoutDeletedAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        await _context.SaveChangesAsync();
        var result = await _repo.GetAllAsync();

        // Assert
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetAllReturnsAllObjectsWithDeletedAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        await _context.SaveChangesAsync();
        var result = await _repo.GetAllAsync(true);

        // Assert
        Assert.Equal(3, result.Count());
    }

    [Fact]
    public async Task GetByIdReturnsDeletedObjectIfDeletedAsync()
    {
        // Arrange
        var test = new TestObject { Name = "test" };
        _context.TestObjects.Add(test);
        _context.SaveChanges();

        // Act
        _context.TestObjects.Remove(test);
        await _context.SaveChangesAsync();
        var result = await _repo.GetByIdAsync(test.Id, true);

        // Assert
        Assert.NotNull(result);
    }

    #endregion

    #region IdentityTests

    [Fact]
    public async Task CreateIdentityUserAsync()
    {
        // Arrange
        var user = new TestUser { UserName = "testuser3" };

        // Act
        var result = await _userManager.CreateAsync(user, "password");

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task FindIdentityUserAsync()
    {
        // Arrange
        var user = new TestUser { UserName = "testuser3" };
        await _userManager.CreateAsync(user, "password");

        // Act
        var result = await _userManager.FindByNameAsync("testuser3");

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdateIdentityUserAsync()
    {
        // Arrange
        var user = new TestUser { UserName = "testuser3" };
        await _userManager.CreateAsync(user, "password");

        // Act
        user.UserName = "testuser4";
        var result = await _userManager.UpdateAsync(user);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task DeleteIdentityUserAsync()
    {
        // Arrange
        var user = new TestUser { UserName = "testuser3" };
        await _userManager.CreateAsync(user, "password");

        // Act
        var result = await _userManager.DeleteAsync(user);

        // Assert
        Assert.True(result.Succeeded);
    }

    [Fact]
    public async Task LoginWorks()
    {
        // Arrange
        var user = new TestUser { UserName = "testuser3" };
        await _userManager.CreateAsync(user, "password");

        // Act
        var result = await _userManager.CheckPasswordAsync(user, "password");

        // Assert
        Assert.True(result);

    }
    #endregion


}
