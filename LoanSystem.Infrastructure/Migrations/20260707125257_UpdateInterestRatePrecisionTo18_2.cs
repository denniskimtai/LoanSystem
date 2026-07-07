using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanSystem.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInterestRatePrecisionTo18_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "loan_products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldPrecision: 5,
                oldScale: 4);

            migrationBuilder.Sql("UPDATE loan_products SET InterestRate = InterestRate * 100 WHERE InterestRate <= 1;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("UPDATE loan_products SET InterestRate = InterestRate / 100;");

            migrationBuilder.AlterColumn<decimal>(
                name: "InterestRate",
                table: "loan_products",
                type: "decimal(5,4)",
                precision: 5,
                scale: 4,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldPrecision: 18,
                oldScale: 2);
        }
    }
}
