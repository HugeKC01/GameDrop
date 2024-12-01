using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDrop.Migrations.GameDrop
{
    /// <inheritdoc />
    public partial class GameDrop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ProfileImageData",
                table: "AspNetUsers",
                type: "varbinary(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileImageType",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageData",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileImageType",
                table: "AspNetUsers");
        }
    }
}
