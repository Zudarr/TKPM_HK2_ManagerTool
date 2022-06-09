using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class UpdateNavigatorDaiLyFromPhieuThuTien : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TenDaiLy",
                table: "DaiLys",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PhieuThuTiens_IdDaiLy",
                table: "PhieuThuTiens",
                column: "IdDaiLy");

            migrationBuilder.CreateIndex(
                name: "IX_DaiLys_TenDaiLy",
                table: "DaiLys",
                column: "TenDaiLy",
                unique: true,
                filter: "[TenDaiLy] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuThuTiens_DaiLys_IdDaiLy",
                table: "PhieuThuTiens",
                column: "IdDaiLy",
                principalTable: "DaiLys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PhieuThuTiens_DaiLys_IdDaiLy",
                table: "PhieuThuTiens");

            migrationBuilder.DropIndex(
                name: "IX_PhieuThuTiens_IdDaiLy",
                table: "PhieuThuTiens");

            migrationBuilder.DropIndex(
                name: "IX_DaiLys_TenDaiLy",
                table: "DaiLys");

            migrationBuilder.AlterColumn<string>(
                name: "TenDaiLy",
                table: "DaiLys",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
