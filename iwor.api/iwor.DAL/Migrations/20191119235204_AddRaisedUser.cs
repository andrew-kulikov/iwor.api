using Microsoft.EntityFrameworkCore.Migrations;

namespace iwor.DAL.Migrations
{
    public partial class AddRaisedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RaisedUserId",
                table: "PriceRaises",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceRaises_RaisedUserId",
                table: "PriceRaises",
                column: "RaisedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceRaises_AspNetUsers_RaisedUserId",
                table: "PriceRaises",
                column: "RaisedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceRaises_AspNetUsers_RaisedUserId",
                table: "PriceRaises");

            migrationBuilder.DropIndex(
                name: "IX_PriceRaises_RaisedUserId",
                table: "PriceRaises");

            migrationBuilder.DropColumn(
                name: "RaisedUserId",
                table: "PriceRaises");
        }
    }
}
