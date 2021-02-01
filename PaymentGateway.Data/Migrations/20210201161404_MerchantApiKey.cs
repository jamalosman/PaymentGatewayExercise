using Microsoft.EntityFrameworkCore.Migrations;

namespace PaymentGateway.Data.Migrations
{
    public partial class MerchantApiKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Merchant_MerchantId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchant",
                table: "Merchant");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Merchant",
                newName: "Merchants");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_MerchantId",
                table: "Payments",
                newName: "IX_Payments_MerchantId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "DECIMAL(19,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "ApiKey",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EncryptionKey",
                table: "Merchants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Merchants_MerchantId",
                table: "Payments",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Merchants_MerchantId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "ApiKey",
                table: "Merchants");

            migrationBuilder.DropColumn(
                name: "EncryptionKey",
                table: "Merchants");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Merchants",
                newName: "Merchant");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_MerchantId",
                table: "Payment",
                newName: "IX_Payment_MerchantId");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payment",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "DECIMAL(19,4)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchant",
                table: "Merchant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Merchant_MerchantId",
                table: "Payment",
                column: "MerchantId",
                principalTable: "Merchant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
