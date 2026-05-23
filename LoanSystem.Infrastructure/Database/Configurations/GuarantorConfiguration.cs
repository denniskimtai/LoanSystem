using LoanSystem.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class GuarantorConfiguration : IEntityTypeConfiguration<Guarantor>
{
    public void Configure(EntityTypeBuilder<Guarantor> builder)
    {
        builder.ToTable("guarantors");

        builder.HasKey(g => g.Id);

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(g => g.IdNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(g => g.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(g => g.AmountGuaranteed)
            .HasPrecision(18, 2);

        builder.Property(g => g.Relationship)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(g => g.Customer)
            .WithMany(c => c.Guarantors)
            .HasForeignKey(g => g.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
