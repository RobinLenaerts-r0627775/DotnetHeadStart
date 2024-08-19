using DotnetHeadStart.DB;

namespace DotnetHeadStart.Exceptions;

public static class DBExtensions
{
    public static void AddBaseModelQueryFilter(this ModelBuilder modelBuilder)
    {
        modelBuilder.Model.GetEntityTypes()
            .Where(e => typeof(BaseModel).IsAssignableFrom(e.ClrType))
            .ToList()
            .ForEach(e =>
            {
                var parameter = Expression.Parameter(e.ClrType, "e");
                var property = Expression.Property(parameter, nameof(BaseModel.IsDeleted));
                var negation = Expression.Not(property);
                var lambda = Expression.Lambda(negation, parameter);

                modelBuilder.Entity(e.ClrType).HasQueryFilter(lambda);

                modelBuilder.Entity(e.ClrType)
                    .HasIndex("IsDeleted");
            });
    }
}
