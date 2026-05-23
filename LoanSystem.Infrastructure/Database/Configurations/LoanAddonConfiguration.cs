using LoanSystem.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class LoanAddonConfiguration : IEntityTypeConfiguration<LoanAddon>
{
    public void Configure(EntityTypeBuilder<LoanAddon> builder)
    {
        builder.ToTable("loan_addons");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Amount)
            .HasPrecision(18, 2);

        builder.HasOne(a => a.Loan)
            .WithMany(l => l.Addons)
            .HasForeignKey(a => a.LoanId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(a => a.AppliedBy)
            .WithMany()
            .HasForeignKey(a => a.AppliedById)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
