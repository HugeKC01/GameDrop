using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDrop.Migrations
{
    /// <inheritdoc />
    public partial class AddKey4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_GameDrop_OrderOrderId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_GameDrop_OrderOrderId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "GameDrop_OrderOrderId",
                table: "OrderDetails");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "GameDrop_OrderOrderId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_GameDrop_OrderOrderId",
                table: "OrderDetails",
                column: "GameDrop_OrderOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_GameDrop_OrderOrderId",
                table: "OrderDetails",
                column: "GameDrop_OrderOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }
    }
}
