using LoanSystem.Domain.Entities.CRM;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class InteractionConfiguration : IEntityTypeConfiguration<Interaction>
{
    public void Configure(EntityTypeBuilder<Interaction> builder)
    {
        builder.ToTable("interactions");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Mode)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.Purpose)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(i => i.OutcomeDetails)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(i => i.OutcomeStatus)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.Tag)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(i => i.PromisedAmount)
            .HasPrecision(18, 2)
            .IsRequired(false);

        builder.Property(i => i.DefaultReason)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(i => i.NextSteps)
            .IsRequired()
            .HasMaxLength(2000);

        builder.Property(i => i.LocationGeo)
            .IsRequired(false)
            .HasMaxLength(200);

        builder.HasOne(i => i.Customer)
            .WithMany(c => c.Interactions)
            .HasForeignKey(i => i.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Loan)
            .WithMany(l => l.Interactions)
            .HasForeignKey(i => i.LoanId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Agent)
            .WithMany()
            .HasForeignKey(i => i.AgentId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
