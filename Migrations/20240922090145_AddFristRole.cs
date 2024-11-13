using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebboardMVC.Migrations
{
    public partial class AddFristRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "71a23f01-1a4f-40ad-b1b6-6a2329c9c9f4", null, "Member", "MEMBER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "9374f95c-5bd3-4032-977b-3e407e324cd8", null, "Admin", "ADMIN" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "71a23f01-1a4f-40ad-b1b6-6a2329c9c9f4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9374f95c-5bd3-4032-977b-3e407e324cd8");
        }
    }
}
