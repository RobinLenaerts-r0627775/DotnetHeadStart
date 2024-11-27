namespace DotnetHeadStart.Tests.Setup;

public class TestSoftDeletableRepository(DbContext context) : BaseRepository<TestObjectSoftDeletable>(context)
{

}
