using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAccountReceivableInstallments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "installment_group_id",
                table: "accounts_receivable",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "installment_number",
                table: "accounts_receivable",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "total_installments",
                table: "accounts_receivable",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "ix_accounts_receivable_installment_group_id",
                table: "accounts_receivable",
                column: "installment_group_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_accounts_receivable_installment_group_id",
                table: "accounts_receivable");

            migrationBuilder.DropColumn(
                name: "installment_group_id",
                table: "accounts_receivable");

            migrationBuilder.DropColumn(
                name: "installment_number",
                table: "accounts_receivable");

            migrationBuilder.DropColumn(
                name: "total_installments",
                table: "accounts_receivable");
        }
    }
}
