using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class fixQuiDinh_IsChanged_To_Changeable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsChanged",
                table: "QuyDinhs",
                newName: "Changeable");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Changeable",
                table: "QuyDinhs",
                newName: "IsChanged");
        }
    }
}
