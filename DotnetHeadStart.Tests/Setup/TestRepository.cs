namespace DotnetHeadStart.Tests.Setup;

public class TestRepository(DbContext context) : BaseRepository<TestObject>(context)
{
}
