using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StocksApplication.Infrastructure.Migrations
{
    public partial class UserTradesRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "SellOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "BuyOrders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_SellOrders_UserId",
                table: "SellOrders",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BuyOrders_UserId",
                table: "BuyOrders",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyOrders_AspNetUsers_UserId",
                table: "BuyOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SellOrders_AspNetUsers_UserId",
                table: "SellOrders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyOrders_AspNetUsers_UserId",
                table: "BuyOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_SellOrders_AspNetUsers_UserId",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_SellOrders_UserId",
                table: "SellOrders");

            migrationBuilder.DropIndex(
                name: "IX_BuyOrders_UserId",
                table: "BuyOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SellOrders");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "BuyOrders");
        }
    }
}
