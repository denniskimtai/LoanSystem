using LoanSystem.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.ToTable("customers");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(c => c.NationalId)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(c => c.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(c => c.PhotoUrl)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(c => c.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(c => c.PhysicalAddress)
            .IsRequired()
            .HasColumnType("nvarchar(max)");

        builder.Property(c => c.HomeGeoLocation)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Town)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.County)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(c => c.PostalAddress)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.CurrentLimit)
            .HasPrecision(18, 2);

        // Indexes
        builder.HasIndex(c => c.NationalId)
            .IsUnique(false);

        builder.HasIndex(c => c.Phone)
            .IsUnique(false);

        // FKs
        builder.HasOne(c => c.Branch)
            .WithMany()
            .HasForeignKey(c => c.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.CreatedBy)
            .WithMany()
            .HasForeignKey(c => c.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.CurrentLo)
            .WithMany()
            .HasForeignKey(c => c.CurrentLoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.CurrentCo)
            .WithMany()
            .HasForeignKey(c => c.CurrentCoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
