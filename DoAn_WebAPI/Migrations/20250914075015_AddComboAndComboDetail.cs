using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddComboAndComboDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_MenuItems_MenuItemID",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemID",
                table: "OrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ComboID",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Combos",
                columns: table => new
                {
                    ComboID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Combos", x => x.ComboID);
                    table.ForeignKey(
                        name: "FK_Combos_Restaurants_RestaurantID",
                        column: x => x.RestaurantID,
                        principalTable: "Restaurants",
                        principalColumn: "RestaurantID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComboDetails",
                columns: table => new
                {
                    ComboDetailID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComboID = table.Column<int>(type: "int", nullable: false),
                    MenuItemID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboDetails", x => x.ComboDetailID);
                    table.ForeignKey(
                        name: "FK_ComboDetails_Combos_ComboID",
                        column: x => x.ComboID,
                        principalTable: "Combos",
                        principalColumn: "ComboID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ComboDetails_MenuItems_MenuItemID",
                        column: x => x.MenuItemID,
                        principalTable: "MenuItems",
                        principalColumn: "MenuItemID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ComboID",
                table: "OrderDetails",
                column: "ComboID");

            migrationBuilder.CreateIndex(
                name: "IX_ComboDetails_ComboID",
                table: "ComboDetails",
                column: "ComboID");

            migrationBuilder.CreateIndex(
                name: "IX_ComboDetails_MenuItemID",
                table: "ComboDetails",
                column: "MenuItemID");

            migrationBuilder.CreateIndex(
                name: "IX_Combos_RestaurantID",
                table: "Combos",
                column: "RestaurantID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Combos_ComboID",
                table: "OrderDetails",
                column: "ComboID",
                principalTable: "Combos",
                principalColumn: "ComboID");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_MenuItems_MenuItemID",
                table: "OrderDetails",
                column: "MenuItemID",
                principalTable: "MenuItems",
                principalColumn: "MenuItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Combos_ComboID",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_MenuItems_MenuItemID",
                table: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ComboDetails");

            migrationBuilder.DropTable(
                name: "Combos");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ComboID",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ComboID",
                table: "OrderDetails");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemID",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_MenuItems_MenuItemID",
                table: "OrderDetails",
                column: "MenuItemID",
                principalTable: "MenuItems",
                principalColumn: "MenuItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
