using Microsoft.EntityFrameworkCore.Migrations;

namespace Billiard.DataLayer.MSSQL.Migrations
{
    public partial class addMoshavereh2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Consulting_AppUsers_UserId",
                table: "Consulting");

            migrationBuilder.DropIndex(
                name: "IX_Consulting_UserId",
                table: "Consulting");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Consulting_UserId",
                table: "Consulting",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Consulting_AppUsers_UserId",
                table: "Consulting",
                column: "UserId",
                principalTable: "AppUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
