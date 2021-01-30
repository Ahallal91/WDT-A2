using Microsoft.EntityFrameworkCore.Migrations;

namespace A2.Migrations
{
    public partial class Addedroles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d7c4629f-d4fd-46c9-a104-704a57b1c12f", "1f2f349a-7d35-49ac-8d78-f726a6ce0b21", "Admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "18cc60d8-08ce-458a-b2ef-d39d39a678ea", "c2c70aeb-01ed-4e49-b83b-b4eb4f116bc2", "Customer", "CUSTOMER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "18cc60d8-08ce-458a-b2ef-d39d39a678ea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d7c4629f-d4fd-46c9-a104-704a57b1c12f");
        }
    }
}
