using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class fixQuyDinh : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "QuyDinhs");

            migrationBuilder.AddColumn<int>(
                name: "GiaTri",
                table: "QuyDinhs",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GiaTri",
                table: "QuyDinhs");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "QuyDinhs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
