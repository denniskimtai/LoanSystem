using LoanSystem.Domain.Entities.Loans;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class PayScheduleConfiguration : IEntityTypeConfiguration<PaySchedule>
{
    public void Configure(EntityTypeBuilder<PaySchedule> builder)
    {
        builder.ToTable("pay_schedule");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Amount)
            .HasPrecision(18, 2);

        builder.Property(p => p.Balance)
            .HasPrecision(18, 2);

        builder.Property(p => p.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne(p => p.Loan)
            .WithMany(l => l.PaySchedules)
            .HasForeignKey(p => p.LoanId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
