using LoanSystem.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class CustomerBusinessInfoConfiguration : IEntityTypeConfiguration<CustomerBusinessInfo>
{
    public void Configure(EntityTypeBuilder<CustomerBusinessInfo> builder)
    {
        builder.ToTable("customer_business_info");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.BusinessName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.BusinessType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.BusinessDirection)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.BusinessGeoLocation)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.CurrentStockValue)
            .HasPrecision(18, 2);

        builder.Property(c => c.WeeklyGrossProfit)
            .HasPrecision(18, 2);

        builder.Property(c => c.WeeklyNetProfit)
            .HasPrecision(18, 2);

        builder.Property(c => c.WeeklyExpenses)
            .HasPrecision(18, 2);

        builder.Property(c => c.LeadType)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.ProposedLimit)
            .HasPrecision(18, 2);

        // One-to-One FK
        builder.HasOne(c => c.Customer)
            .WithOne(cust => cust.BusinessInfo)
            .HasForeignKey<CustomerBusinessInfo>(c => c.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
