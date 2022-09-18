using Microsoft.EntityFrameworkCore.Migrations;

namespace Billiard.DataLayer.MSSQL.Migrations
{
    public partial class addBlogPostCategoryIdeblog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPostCategory_BlogPostCategoryId",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<int>(
                name: "BlogPostCategoryId",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogPostCategory_BlogPostCategoryId",
                table: "BlogPosts",
                column: "BlogPostCategoryId",
                principalTable: "BlogPostCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPosts_BlogPostCategory_BlogPostCategoryId",
                table: "BlogPosts");

            migrationBuilder.AlterColumn<int>(
                name: "BlogPostCategoryId",
                table: "BlogPosts",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "BlogPosts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPosts_BlogPostCategory_BlogPostCategoryId",
                table: "BlogPosts",
                column: "BlogPostCategoryId",
                principalTable: "BlogPostCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
