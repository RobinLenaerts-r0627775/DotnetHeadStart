namespace DotnetHeadStart.Tests;

public class TestRepository(DbContext context) : BaseRepository<TestObject>(context)
{
}
