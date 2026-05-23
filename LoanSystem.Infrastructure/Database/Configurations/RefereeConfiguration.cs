using LoanSystem.Domain.Entities.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class RefereeConfiguration : IEntityTypeConfiguration<Referee>
{
    public void Configure(EntityTypeBuilder<Referee> builder)
    {
        builder.ToTable("referees");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(r => r.Phone)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(r => r.PhysicalAddress)
            .IsRequired()
            .HasMaxLength(300);

        builder.Property(r => r.Relationship)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasOne(r => r.Customer)
            .WithMany(c => c.Referees)
            .HasForeignKey(r => r.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
