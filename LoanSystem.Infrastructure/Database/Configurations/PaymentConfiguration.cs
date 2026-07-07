using LoanSystem.Domain.Entities.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.ToTable("payments");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);

        builder.Property(p => p.TransactionCode)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasIndex(p => p.TransactionCode)
            .IsUnique();

        builder.Property(p => p.MpesaRef)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.PayMethod)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(p => p.RecordType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne(p => p.Loan)
            .WithMany(l => l.Payments)
            .HasForeignKey(p => p.LoanId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Customer)
            .WithMany(c => c.Payments)
            .HasForeignKey(p => p.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.RecordedBy)
            .WithMany()
            .HasForeignKey(p => p.RecordedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
