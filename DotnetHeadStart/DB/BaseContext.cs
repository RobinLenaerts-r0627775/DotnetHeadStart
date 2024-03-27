using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
namespace DotnetHeadStart;

/// <summary>
/// Implementation of the DbContext class. Base functionalities: 
/// - Automatically set the CreatedAt, ModifiedAt and DeletedAt properties of the BaseModel entities.
/// </summary>
public class BaseContext(DbContextOptions options) : DbContext(options)
{



    protected override void OnModelCreating(ModelBuilder modelBuilder)
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