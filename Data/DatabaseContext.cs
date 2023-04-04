namespace DotnetHeadStart.Data;

public class DataBaseContext : DbContext
{
    ILogger _logger;
    // MySQL database context
    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
    {
    }
}