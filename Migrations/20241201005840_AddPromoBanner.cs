using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameDrop.Migrations
{
    /// <inheritdoc />
    public partial class AddPromoBanner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PromoBanners",
                columns: table => new
                {
                    PromoBannerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromoBannerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoBannerDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromoBannerImageData = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    PromoBannerImageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BannerType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PromoBanners", x => x.PromoBannerId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PromoBanners");
        }
    }
}
