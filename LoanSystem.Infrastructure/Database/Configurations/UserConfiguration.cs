using LoanSystem.Domain.Entities.Identity;
using LoanSystem.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LoanSystem.Infrastructure.Database.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.Property(u => u.FullName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(u => u.Role)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder.HasOne(u => u.Branch)
            .WithMany(b => b.Users)
            .HasForeignKey(u => u.BranchId)
            .OnDelete(DeleteBehavior.Restrict);

        // Seed initial admin user using completely static anonymous object
        // to prevent dynamic evaluation on migrations validation (e.g. DateTime.UtcNow, dynamic hash salts)
        builder.HasData(new
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            UserName = "denniskimtai1@gmail.com",
            NormalizedUserName = "DENNISKIMTAI1@GMAIL.COM",
            Email = "denniskimtai1@gmail.com",
            NormalizedEmail = "DENNISKIMTAI1@GMAIL.COM",
            EmailConfirmed = true,
            // Pre-computed hash of "Admin@123" for user "denniskimtai1@gmail.com"
            PasswordHash = "AQAAAAIAAYagAAAAEH5JhbKp1v2ApugyWajFv6BoIiJcYYwW4tL8RgQiG8l+9VpY7P96Tc5JbrLYZ6AJig==",
            SecurityStamp = "33333333-3333-3333-3333-333333333333",
            ConcurrencyStamp = "44444444-4444-4444-4444-444444444444",
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = false,
            AccessFailedCount = 0,
            FullName = "Dennis Kimtai",
            Role = UserRole.Admin,
            BranchId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            IsActive = true,
            CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
        });
    }
}
