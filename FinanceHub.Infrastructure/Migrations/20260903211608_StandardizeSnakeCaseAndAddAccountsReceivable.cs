using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FinanceHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StandardizeSnakeCaseAndAddAccountsReceivable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bills_Categories_CategoryId",
                table: "Bills");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationLogs_Bills_BillId",
                table: "NotificationLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bills",
                table: "Bills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationLogs",
                table: "NotificationLogs");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameTable(
                name: "Bills",
                newName: "bills");

            migrationBuilder.RenameTable(
                name: "NotificationLogs",
                newName: "notification_logs");

            migrationBuilder.RenameColumn(
                name: "Wallet",
                table: "users",
                newName: "wallet");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "users",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "users",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "VerifcationCode",
                table: "users",
                newName: "verification_code");

            migrationBuilder.RenameColumn(
                name: "SecondName",
                table: "users",
                newName: "second_name");

            migrationBuilder.RenameColumn(
                name: "DateBirth",
                table: "users",
                newName: "date_birth");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "users",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "ConfirmEmail",
                table: "users",
                newName: "confirm_email");

            migrationBuilder.RenameColumn(
                name: "CodeExpiration",
                table: "users",
                newName: "code_expiration");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "users",
                newName: "IX_users_email");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "categories",
                newName: "user_id");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "categories",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoryType",
                table: "categories",
                newName: "category_type");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_UserId",
                table: "categories",
                newName: "IX_categories_user_id");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "bills",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "bills",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bills",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "DatePayment",
                table: "bills",
                newName: "date_payment");

            migrationBuilder.RenameColumn(
                name: "DateDue",
                table: "bills",
                newName: "date_due");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "bills",
                newName: "created_at");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "bills",
                newName: "category_id");

            migrationBuilder.RenameColumn(
                name: "BillStatus",
                table: "bills",
                newName: "bill_status");

            migrationBuilder.RenameIndex(
                name: "IX_Bills_CategoryId",
                table: "bills",
                newName: "IX_bills_category_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "notification_logs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SentAt",
                table: "notification_logs",
                newName: "sent_at");

            migrationBuilder.RenameColumn(
                name: "NotificationType",
                table: "notification_logs",
                newName: "notification_type");

            migrationBuilder.RenameColumn(
                name: "BillId",
                table: "notification_logs",
                newName: "bill_id");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationLogs_BillId_NotificationType",
                table: "notification_logs",
                newName: "IX_notification_logs_bill_id_notification_type");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bills",
                table: "bills",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_notification_logs",
                table: "notification_logs",
                column: "id");

            migrationBuilder.CreateTable(
                name: "accounts_receivable",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    description = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    value = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    receive_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    user_id = table.Column<int>(type: "integer", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts_receivable", x => x.id);
                    table.ForeignKey(
                        name: "FK_accounts_receivable_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accounts_receivable_user_id",
                table: "accounts_receivable",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_bills_categories_category_id",
                table: "bills",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categories_users_user_id",
                table: "categories",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notification_logs_bills_bill_id",
                table: "notification_logs",
                column: "bill_id",
                principalTable: "bills",
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
                name: "FK_categories_users_user_id",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "FK_notification_logs_bills_bill_id",
                table: "notification_logs");

            migrationBuilder.DropTable(
                name: "accounts_receivable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categories",
                table: "categories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bills",
                table: "bills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_notification_logs",
                table: "notification_logs");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "bills",
                newName: "Bills");

            migrationBuilder.RenameTable(
                name: "notification_logs",
                newName: "NotificationLogs");

            migrationBuilder.RenameColumn(
                name: "wallet",
                table: "Users",
                newName: "Wallet");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Users",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "verification_code",
                table: "Users",
                newName: "VerifcationCode");

            migrationBuilder.RenameColumn(
                name: "second_name",
                table: "Users",
                newName: "SecondName");

            migrationBuilder.RenameColumn(
                name: "date_birth",
                table: "Users",
                newName: "DateBirth");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Users",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "confirm_email",
                table: "Users",
                newName: "ConfirmEmail");

            migrationBuilder.RenameColumn(
                name: "code_expiration",
                table: "Users",
                newName: "CodeExpiration");

            migrationBuilder.RenameIndex(
                name: "IX_users_email",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "user_id",
                table: "Categories",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Categories",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "category_type",
                table: "Categories",
                newName: "CategoryType");

            migrationBuilder.RenameIndex(
                name: "IX_categories_user_id",
                table: "Categories",
                newName: "IX_Categories_UserId");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Bills",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Bills",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Bills",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "date_payment",
                table: "Bills",
                newName: "DatePayment");

            migrationBuilder.RenameColumn(
                name: "date_due",
                table: "Bills",
                newName: "DateDue");

            migrationBuilder.RenameColumn(
                name: "created_at",
                table: "Bills",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Bills",
                newName: "CategoryId");

            migrationBuilder.RenameColumn(
                name: "bill_status",
                table: "Bills",
                newName: "BillStatus");

            migrationBuilder.RenameIndex(
                name: "IX_bills_category_id",
                table: "Bills",
                newName: "IX_Bills_CategoryId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "NotificationLogs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "sent_at",
                table: "NotificationLogs",
                newName: "SentAt");

            migrationBuilder.RenameColumn(
                name: "notification_type",
                table: "NotificationLogs",
                newName: "NotificationType");

            migrationBuilder.RenameColumn(
                name: "bill_id",
                table: "NotificationLogs",
                newName: "BillId");

            migrationBuilder.RenameIndex(
                name: "IX_notification_logs_bill_id_notification_type",
                table: "NotificationLogs",
                newName: "IX_NotificationLogs_BillId_NotificationType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bills",
                table: "Bills",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationLogs",
                table: "NotificationLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bills_Categories_CategoryId",
                table: "Bills",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Users_UserId",
                table: "Categories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationLogs_Bills_BillId",
                table: "NotificationLogs",
                column: "BillId",
                principalTable: "Bills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
