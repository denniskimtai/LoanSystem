using LoanSystem.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class LoanDeductionConfiguration : IEntityTypeConfiguration<LoanDeduction>
{
    public void Configure(EntityTypeBuilder<LoanDeduction> builder)
    {
        builder.ToTable("loan_deductions");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Amount)
            .HasPrecision(18, 2);

        builder.HasOne(d => d.Loan)
            .WithMany(l => l.Deductions)
            .HasForeignKey(d => d.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.AppliedBy)
            .WithMany()
            .HasForeignKey(d => d.AppliedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
