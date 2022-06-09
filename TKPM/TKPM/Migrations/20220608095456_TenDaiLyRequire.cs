using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class TenDaiLyRequire : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DaiLys_TenDaiLy",
                table: "DaiLys");

            migrationBuilder.AlterColumn<string>(
                name: "TenDaiLy",
                table: "DaiLys",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaiLys_TenDaiLy",
                table: "DaiLys",
                column: "TenDaiLy",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DaiLys_TenDaiLy",
                table: "DaiLys");

            migrationBuilder.AlterColumn<string>(
                name: "TenDaiLy",
                table: "DaiLys",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_DaiLys_TenDaiLy",
                table: "DaiLys",
                column: "TenDaiLy",
                unique: true,
                filter: "[TenDaiLy] IS NOT NULL");
        }
    }
}
