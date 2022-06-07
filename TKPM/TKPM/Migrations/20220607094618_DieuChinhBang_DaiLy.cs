using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class DieuChinhBang_DaiLy : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoCuoi",
                table: "DaiLys");

            migrationBuilder.DropColumn(
                name: "NoDau",
                table: "DaiLys");

            migrationBuilder.RenameColumn(
                name: "PhatSinh",
                table: "DaiLys",
                newName: "NoHienTai");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoHienTai",
                table: "DaiLys",
                newName: "PhatSinh");

            migrationBuilder.AddColumn<int>(
                name: "NoCuoi",
                table: "DaiLys",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NoDau",
                table: "DaiLys",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
