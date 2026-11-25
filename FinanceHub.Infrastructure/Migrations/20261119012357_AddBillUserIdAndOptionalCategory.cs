using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinanceHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddBillUserIdAndOptionalCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills");

            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "bills",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            // 1) entra como nullable para permitir o backfill
            migrationBuilder.AddColumn<int>(
                name: "user_id",
                table: "bills",
                type: "integer",
                nullable: true);

            // 2) dono da conta vem da categoria atual
            migrationBuilder.Sql("""
                UPDATE bills AS b
                SET user_id = c.user_id
                FROM categories AS c
                WHERE c.id = b.category_id;
                """);

            // 3) só depois de preenchido vira obrigatório
            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "bills",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_bills_user_id",
                table: "bills",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_bills_users_user_id",
                table: "bills",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills");

            migrationBuilder.DropForeignKey(
                name: "FK_bills_users_user_id",
                table: "bills");

            migrationBuilder.DropIndex(
                name: "ix_bills_user_id",
                table: "bills");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "bills");

            // evita falha ao tornar category_id obrigatório novamente
            migrationBuilder.Sql("DELETE FROM bills WHERE category_id IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "category_id",
                table: "bills",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
