using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeBillDatePaymentPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePayment",
                table: "Bills",
                type: "timestamp(0) without time zone",
                precision: 0,
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DatePayment",
                table: "Bills",
                type: "timestamp without time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp(0) without time zone",
                oldPrecision: 0,
                oldNullable: true);
        }
    }
}
