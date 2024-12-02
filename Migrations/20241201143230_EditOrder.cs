using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDrop.Migrations
{
    /// <inheritdoc />
    public partial class EditOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderAddress",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderEmail",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "OrderPhone",
                table: "Orders",
                newName: "UserAddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserAddressId",
                table: "Orders",
                newName: "OrderPhone");

            migrationBuilder.AddColumn<string>(
                name: "OrderAddress",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderEmail",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
