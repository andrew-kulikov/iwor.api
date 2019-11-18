using Microsoft.EntityFrameworkCore.Migrations;

namespace iwor.DAL.Migrations
{
    public partial class RenameUserIdInAuction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Auctions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Auctions",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
