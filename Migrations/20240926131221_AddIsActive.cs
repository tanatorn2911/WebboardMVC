using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebboardMVC.Migrations
{
    public partial class AddIsActive : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "AspNetUsers", 
                nullable: false,
                defaultValue: true); 
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2316f809-4060-438e-898b-e3e1c0806fab");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c753e413-b220-455b-badd-d369bbb3f3bf");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "18c77609-dd09-4912-8a21-1bba971566b4", null, "Member", "MEMBER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "cdd6f0a6-3dad-4494-8219-3ed56e567328", null, "Admin", "ADMIN" });
        }
    }
}
