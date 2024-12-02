using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDrop.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "OrderDetails",
                newName: "OrderQuantity");

            migrationBuilder.AddColumn<string>(
                name: "OrderProductName",
                table: "OrderDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderProductName",
                table: "OrderDetails");

            migrationBuilder.RenameColumn(
                name: "OrderQuantity",
                table: "OrderDetails",
                newName: "Quantity");
        }
    }
}
