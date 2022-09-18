using Microsoft.EntityFrameworkCore.Migrations;

namespace Billiard.DataLayer.MSSQL.Migrations
{
    public partial class dlaparat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AparatVideoFileName",
                table: "BlogPosts",
                newName: "VideoFileName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VideoFileName",
                table: "BlogPosts",
                newName: "AparatVideoFileName");
        }
    }
}
