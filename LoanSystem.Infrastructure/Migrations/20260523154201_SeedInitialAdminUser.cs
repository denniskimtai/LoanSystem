using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "branches",
                columns: new[] { "Id", "CreatedAt", "IsDeleted", "Location", "Name", "UpdatedAt" },
                values: new object[] { new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Main Headquarters", "Head Office", null });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "Id", "AccessFailedCount", "BranchId", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FullName", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "Role", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { new Guid("22222222-2222-2222-2222-222222222222"), 0, new Guid("11111111-1111-1111-1111-111111111111"), "44444444-4444-4444-4444-444444444444", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "denniskimtai1@gmail.com", true, "Dennis Kimtai", true, false, null, "DENNISKIMTAI1@GMAIL.COM", "DENNISKIMTAI1@GMAIL.COM", "AQAAAAIAAYagAAAAEH5JhbKp1v2ApugyWajFv6BoIiJcYYwW4tL8RgQiG8l+9VpY7P96Tc5JbrLYZ6AJig==", null, false, "Admin", "33333333-3333-3333-3333-333333333333", false, "denniskimtai1@gmail.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "branches",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));
        }
    }
}
