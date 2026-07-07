using LoanSystem.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class LoanProductConfiguration : IEntityTypeConfiguration<LoanProduct>
{
    public void Configure(EntityTypeBuilder<LoanProduct> builder)
    {
        builder.ToTable("loan_products");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(l => l.Name)
            .IsUnique();

        builder.Property(l => l.MinAmount)
            .HasPrecision(18, 2);

        builder.Property(l => l.MaxAmount)
            .HasPrecision(18, 2);

        builder.Property(l => l.InterestRate)
            .HasPrecision(18, 2);
    }
}
