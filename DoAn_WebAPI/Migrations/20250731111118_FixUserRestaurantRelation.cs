using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class FixUserRestaurantRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restaurants_UserID",
                table: "Restaurants");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_UserID",
                table: "Restaurants",
                column: "UserID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Restaurants_UserID",
                table: "Restaurants");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_UserID",
                table: "Restaurants",
                column: "UserID");
        }
    }
}
