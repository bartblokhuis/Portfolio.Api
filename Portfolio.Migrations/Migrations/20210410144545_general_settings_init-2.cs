using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.Migrations.Migrations
{
    public partial class general_settings_init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GeneralSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LandingTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LandingDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallToActionText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GithubUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StackOverFlowUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FooterText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowCopyRightInFooter = table.Column<bool>(type: "bit", nullable: false),
                    FooterTextBetweenCopyRightAndYear = table.Column<bool>(type: "bit", nullable: false),
                    ShowContactMeForm = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralSettings", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneralSettings");
        }
    }
}
