using Microsoft.EntityFrameworkCore.Migrations;

namespace Billiard.DataLayer.MSSQL.Migrations
{
    public partial class addIsdeleteblog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "BlogPosts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "BlogPostLikes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "BlogPostComments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "BlogPostCategory",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "BlogPostLikes");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "BlogPostComments");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "BlogPostCategory");
        }
    }
}
