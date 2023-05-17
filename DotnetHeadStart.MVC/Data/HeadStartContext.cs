namespace DotnetHeadStart.MVC.Data;

public class HeadStartContext : DbContext
{
    readonly ILogger _logger;
    //public DbSet<ProfessionalExperience> ProfessionalExperiences { get; set; }
    public HeadStartContext(DbContextOptions<HeadStartContext> options, ILogger logger) : base(options)
    {
        _logger = logger;
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // modelBuilder.Entity<ProfessionalExperience>()
        //     .Property(p => p.TechStack)
        //     .HasConversion(
        //         v => string.Join(',', v),
        //         v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList());
    }
    public override int SaveChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
        foreach (var entityEntry in entries)
        {
            _logger.Information("Saving entity of type {EntityType} with state {State} to database",
                entityEntry.Entity.GetType().Name, entityEntry.State);
            switch (entityEntry.State)
            {
                case EntityState.Added:
                    ((BaseModel)entityEntry.Entity).CreatedAt = DateTime.Now;
                    break;
                case EntityState.Modified:
                    ((BaseModel)entityEntry.Entity).ModifiedAt = DateTime.Now;
                    break;
                case EntityState.Deleted:
                    ((BaseModel)entityEntry.Entity).DeletedAt = DateTime.Now;
                    entityEntry.State = EntityState.Modified;
                    break;
            }
        }
        return base.SaveChanges();
    }
}