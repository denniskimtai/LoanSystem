using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Primitives;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace LoanSystem.Infrastructure.Database;

public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Branch> Branches { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<LoanSystem.Domain.Entities.Customers.Customer> Customers { get; set; }
    public DbSet<LoanSystem.Domain.Entities.Customers.CustomerBusinessInfo> CustomerBusinessInfos { get; set; }
    public DbSet<LoanSystem.Domain.Entities.Customers.CustomerSecondaryInfo> CustomerSecondaryInfos { get; set; }
    public DbSet<LoanSystem.Domain.Entities.Customers.Guarantor> Guarantors { get; set; }
    public DbSet<LoanSystem.Domain.Entities.Customers.Referee> Referees { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all IEntityTypeConfiguration from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        // Global configuration for BaseEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                // Soft delete global query filter
                var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                var propertyMethodInfo = typeof(Microsoft.EntityFrameworkCore.EF).GetMethod(nameof(Microsoft.EntityFrameworkCore.EF.Property))!.MakeGenericMethod(typeof(bool));
                var isDeletedProperty = System.Linq.Expressions.Expression.Call(propertyMethodInfo, parameter, System.Linq.Expressions.Expression.Constant("IsDeleted"));
                var compareExpression = System.Linq.Expressions.Expression.MakeBinary(System.Linq.Expressions.ExpressionType.Equal, isDeletedProperty, System.Linq.Expressions.Expression.Constant(false));
                var lambda = System.Linq.Expressions.Expression.Lambda(compareExpression, parameter);
                
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Global configuration for decimal precision (financial tracking)
        configurationBuilder
            .Properties<decimal>()
            .HavePrecision(18, 4);
            
        configurationBuilder
            .Properties<decimal?>()
            .HavePrecision(18, 4);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    // CreatedAt is initialized to UtcNow in BaseEntity constructor
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.UpdatedAt).CurrentValue = DateTime.UtcNow;
                    break;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
