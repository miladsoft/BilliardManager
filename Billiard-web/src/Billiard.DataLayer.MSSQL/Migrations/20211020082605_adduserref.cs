using Microsoft.EntityFrameworkCore.Migrations;

namespace Billiard.DataLayer.MSSQL.Migrations
{
    public partial class adduserref : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Referrer",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YourRefferCode",
                table: "AppUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Referrer",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "YourRefferCode",
                table: "AppUsers");
        }
    }
}
