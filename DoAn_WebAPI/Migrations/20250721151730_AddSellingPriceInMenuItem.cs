using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSellingPriceInMenuItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "MenuItems",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "MenuItems");
        }
    }
}
