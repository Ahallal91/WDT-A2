using Microsoft.EntityFrameworkCore.Migrations;

namespace A2.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_DestAccountAccountNumber",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_DestAccountAccountNumber",
                table: "Transaction");

            migrationBuilder.DropColumn(
                name: "DestAccountAccountNumber",
                table: "Transaction");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestinationAccount",
                table: "Transaction",
                column: "DestinationAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_DestinationAccount",
                table: "Transaction",
                column: "DestinationAccount",
                principalTable: "Account",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_DestinationAccount",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_DestinationAccount",
                table: "Transaction");

            migrationBuilder.AddColumn<int>(
                name: "DestAccountAccountNumber",
                table: "Transaction",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_DestAccountAccountNumber",
                table: "Transaction",
                column: "DestAccountAccountNumber");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_DestAccountAccountNumber",
                table: "Transaction",
                column: "DestAccountAccountNumber",
                principalTable: "Account",
                principalColumn: "AccountNumber",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
