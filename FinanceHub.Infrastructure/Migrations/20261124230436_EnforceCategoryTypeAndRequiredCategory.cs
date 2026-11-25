using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class EnforceCategoryTypeAndRequiredCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Valores antigos do enum (2, 3, 4) passam a ser Expense = 1
            migrationBuilder.Sql("""
                UPDATE categories SET category_type = 1;
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills");

            migrationBuilder.DropIndex(
                name: "IX_categories_user_id",
                table: "categories");

            // Categoria passa a ser obrigatória na bill
            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "bills",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ux_categories_user_id_name",
                table: "categories",
                columns: new[] { "user_id", "name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id");

            // FK rígida, mas checada no commit (resolve o cascade de usuário)
            migrationBuilder.Sql("""
                ALTER TABLE bills
                ALTER CONSTRAINT "FK_bills_categories_category_id"
                DEFERRABLE INITIALLY DEFERRED;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills");

            migrationBuilder.DropIndex(
                name: "ux_categories_user_id_name",
                table: "categories");

            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "bills",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_categories_user_id",
                table: "categories",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
