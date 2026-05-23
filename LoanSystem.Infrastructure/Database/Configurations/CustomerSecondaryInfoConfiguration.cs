using LoanSystem.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class CustomerSecondaryInfoConfiguration : IEntityTypeConfiguration<CustomerSecondaryInfo>
{
    public void Configure(EntityTypeBuilder<CustomerSecondaryInfo> builder)
    {
        builder.ToTable("customer_secondary_info");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.MaritalStatus)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.Estate)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.HouseNumber)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Ownership)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.RentAmount)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(c => c.HomeAssetValue)
            .HasPrecision(18, 2);

        builder.Property(c => c.NearestLandmark)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.GeoLocation)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.HeardVia)
            .IsRequired()
            .HasMaxLength(100);

        // One-to-One FK
        builder.HasOne(c => c.Customer)
            .WithOne(cust => cust.SecondaryInfo)
            .HasForeignKey<CustomerSecondaryInfo>(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
