using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Promotions_PromotionID",
                table: "PromoCodes");

            migrationBuilder.DropTable(
                name: "Promotions");

            migrationBuilder.DropTable(
                name: "TableReservation");

            migrationBuilder.DropIndex(
                name: "IX_PromoCodes_PromotionID",
                table: "PromoCodes");

            migrationBuilder.DropColumn(
                name: "PromotionID",
                table: "PromoCodes");

            migrationBuilder.CreateTable(
                name: "BestSellingItemResults",
                columns: table => new
                {
                    BestSellingItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "OrderCountResults",
                columns: table => new
                {
                    OrderCountToday = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RevenueTodayResults",
                columns: table => new
                {
                    RevenueToday = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "RevenueWeekResults",
                columns: table => new
                {
                    DayOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Revenue = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "TopSellingItemMonthlyResults",
                columns: table => new
                {
                    ItemName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantitySold = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BestSellingItemResults");

            migrationBuilder.DropTable(
                name: "OrderCountResults");

            migrationBuilder.DropTable(
                name: "RevenueTodayResults");

            migrationBuilder.DropTable(
                name: "RevenueWeekResults");

            migrationBuilder.DropTable(
                name: "TopSellingItemMonthlyResults");

            migrationBuilder.AddColumn<int>(
                name: "PromotionID",
                table: "PromoCodes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Promotions",
                columns: table => new
                {
                    PromotionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountPercent = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Promotions", x => x.PromotionID);
                    table.ForeignKey(
                        name: "FK_Promotions_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TableReservation",
                columns: table => new
                {
                    ReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NumberOfGuests = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpecialRequests = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TableReservation", x => x.ReservationID);
                    table.ForeignKey(
                        name: "FK_TableReservation_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TableReservation_Users_UserID",
                        column: x => x.UserID,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PromoCodes_PromotionID",
                table: "PromoCodes",
                column: "PromotionID");

            migrationBuilder.CreateIndex(
                name: "IX_Promotions_RestaurantID",
                table: "Promotions",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_TableReservation_RestaurantID",
                table: "TableReservation",
                column: "RestaurantID");

            migrationBuilder.CreateIndex(
                name: "IX_TableReservation_UserID",
                table: "TableReservation",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Promotions_PromotionID",
                table: "PromoCodes",
                column: "PromotionID",
                principalTable: "Promotions",
                principalColumn: "PromotionID");
        }
    }
}
