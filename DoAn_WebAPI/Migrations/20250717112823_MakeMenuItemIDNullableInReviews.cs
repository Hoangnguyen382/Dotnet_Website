using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class MakeMenuItemIDNullableInReviews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_MenuItems_MenuItemID",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemID",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_MenuItems_MenuItemID",
                table: "Reviews",
                column: "MenuItemID",
                principalTable: "MenuItems",
                principalColumn: "MenuItemID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_MenuItems_MenuItemID",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "MenuItemID",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_MenuItems_MenuItemID",
                table: "Reviews",
                column: "MenuItemID",
                principalTable: "MenuItems",
                principalColumn: "MenuItemID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
