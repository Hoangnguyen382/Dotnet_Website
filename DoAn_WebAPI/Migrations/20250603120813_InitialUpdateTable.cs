using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialUpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Restaurants_RestaurantID",
                table: "Promotions");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Promotions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "PromoCodes",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PromoCodes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DiscountPercent",
                table: "PromoCodes",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GiftMenuItemID",
                table: "PromoCodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderAmount",
                table: "PromoCodes",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MinQuantity",
                table: "PromoCodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromotionID",
                table: "PromoCodes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RestaurantID",
                table: "PromoCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PromoCodes",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "PromoCodes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Orders",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PromoCodeID",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_PromotionID",
                table: "PromoCodes",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_RestaurantID",
                table: "PromoCodes",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PromoCodeID",
                table: "Orders",
                column: "PromoCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PromoCodes_PromoCodeID",
                table: "Orders",
                column: "PromoCodeID",
                principalTable: "PromoCodes",
                principalColumn: "PromoCodeID");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Promotions_PromotionID",
                table: "PromoCodes",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "PromotionID");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Restaurants_RestaurantID",
                table: "PromoCodes",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "RestaurantID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Restaurants_RestaurantID",
                table: "Promotions",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "RestaurantID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PromoCodes_PromoCodeID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Promotions_PromotionID",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Restaurants_RestaurantID",
                table: "PromoCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Promotions_Restaurants_RestaurantID",
                table: "Promotions");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_PromotionID",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_RestaurantID",
                table: "PromoCodes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PromoCodeID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DiscountPercent",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "GiftMenuItemID",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "MinOrderAmount",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "MinQuantity",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PromotionID",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "RestaurantID",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PromoCodeID",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantID",
                table: "Promotions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountAmount",
                table: "PromoCodes",
                type: "decimal(10,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "PromoCodes",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Promotions_Restaurants_RestaurantID",
                table: "Promotions",
                column: "RestaurantID",
                principalTable: "Restaurants",
                principalColumn: "RestaurantID");
        }
    }
}
