using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace iwor.DAL.Migrations
{
    public partial class RemoveClosingFromAuction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Closings_AuctionId",
                table: "Closings");

            migrationBuilder.DropColumn(
                name: "ClosingId",
                table: "Auctions");

            migrationBuilder.CreateIndex(
                name: "IX_Closings_AuctionId",
                table: "Closings",
                column: "AuctionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Closings_AuctionId",
                table: "Closings");

            migrationBuilder.AddColumn<Guid>(
                name: "ClosingId",
                table: "Auctions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Closings_AuctionId",
                table: "Closings",
                column: "AuctionId",
                unique: true);
        }
    }
}
