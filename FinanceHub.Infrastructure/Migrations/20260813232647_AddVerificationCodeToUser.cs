using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationCodeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CodeExpiration",
                table: "Users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerifcationCode",
                table: "Users",
                type: "character varying(4)",
                maxLength: 4,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CodeExpiration",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "VerifcationCode",
                table: "Users");
        }
    }
}
