using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoAn_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpDateChatTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_OrderId",
                table: "Conversations",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Orders_OrderId",
                table: "Conversations",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Orders_OrderId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_OrderId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Conversations");
        }
    }
}
