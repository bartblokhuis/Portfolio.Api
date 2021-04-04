using Microsoft.EntityFrameworkCore.Migrations;

namespace Portfolio.Migrations.Migrations.AuthenticationDb
{
    public partial class authenticationdataseed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "8e445865-a24d-4543-a6c6-9443d048cdb9", 0, "1d2df4cb-e2f2-44ab-8e5a-0a320e9730dc", null, false, false, null, null, "ADMIN", "AQAAAAEAACcQAAAAEBkliPljyrk+GvxAqA+5xYOcz1P42w75q1yr/zpPxQ+ewDel2s9EBAlkVFe2FkksKw==", null, false, "862e25f7-a4bc-49c5-a1e7-bde96ba49b53", false, "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "8e445865-a24d-4543-a6c6-9443d048cdb9");
        }
    }
}
