using LoanSystem.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class LoanConfiguration : IEntityTypeConfiguration<Loan>
{
    public void Configure(EntityTypeBuilder<Loan> builder)
    {
        builder.ToTable("loans");

        builder.HasKey(l => l.Id);

        builder.Property(l => l.Code)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(l => l.Code)
            .IsUnique();

        builder.Property(l => l.Principal)
            .HasPrecision(18, 2);

        builder.Property(l => l.AddOnsTotal)
            .HasPrecision(18, 2);

        builder.Property(l => l.DeductionsTotal)
            .HasPrecision(18, 2);

        builder.Property(l => l.RepayableTotal)
            .HasPrecision(18, 2);

        builder.Property(l => l.RepaidTotal)
            .HasPrecision(18, 2);

        builder.Property(l => l.Balance)
            .HasPrecision(18, 2);

        builder.Property(l => l.InterestAmount)
            .HasPrecision(18, 2);

        builder.Property(l => l.PenaltyAmount)
            .HasPrecision(18, 2);

        builder.Property(l => l.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(l => l.Stage)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(l => l.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.Property(l => l.MpesaCode)
            .IsRequired(false)
            .HasMaxLength(50);

        // FK relationships
        builder.HasOne(l => l.Customer)
            .WithMany(c => c.Loans)
            .HasForeignKey(l => l.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Product)
            .WithMany()
            .HasForeignKey(l => l.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Branch)
            .WithMany()
            .HasForeignKey(l => l.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Lo)
            .WithMany()
            .HasForeignKey(l => l.LoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.Co)
            .WithMany()
            .HasForeignKey(l => l.CoId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(l => l.CreatedBy)
            .WithMany()
            .HasForeignKey(l => l.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
