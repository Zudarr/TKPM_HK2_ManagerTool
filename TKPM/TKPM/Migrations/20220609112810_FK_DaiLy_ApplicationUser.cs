using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class FK_DaiLy_ApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MaNguoiQuanLy",
                table: "DaiLys",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DaiLys_MaNguoiQuanLy",
                table: "DaiLys",
                column: "MaNguoiQuanLy");

            migrationBuilder.AddForeignKey(
                name: "FK_DaiLys_AspNetUsers_MaNguoiQuanLy",
                table: "DaiLys",
                column: "MaNguoiQuanLy",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DaiLys_AspNetUsers_MaNguoiQuanLy",
                table: "DaiLys");

            migrationBuilder.DropIndex(
                name: "IX_DaiLys_MaNguoiQuanLy",
                table: "DaiLys");

            migrationBuilder.AlterColumn<string>(
                name: "MaNguoiQuanLy",
                table: "DaiLys",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
