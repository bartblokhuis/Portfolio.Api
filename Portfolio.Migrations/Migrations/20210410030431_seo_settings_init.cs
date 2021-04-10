using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.Migrations.Migrations
{
    public partial class seo_settings_init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeoSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultMetaKeywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DefaultMetaDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UseTwitterMetaTags = table.Column<bool>(type: "bit", nullable: false),
                    UseOpenGraphMetaTags = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeoSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeoSettings");
        }
    }
}
