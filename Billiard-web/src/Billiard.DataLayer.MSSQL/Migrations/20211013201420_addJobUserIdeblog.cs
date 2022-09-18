using Microsoft.EntityFrameworkCore.Migrations;

namespace Billiard.DataLayer.MSSQL.Migrations
{
    public partial class addJobUserIdeblog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserDescription",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Job",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "UserDescription",
                table: "AppUsers");
        }
    }
}
