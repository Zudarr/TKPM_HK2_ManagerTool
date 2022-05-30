using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class addHangHoa_updatePhieuXuatHang_ChiTietXuatHang_Models : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietXuatHang_PhieuXuatHangs_PhieuXuatHangID",
                table: "ChiTietXuatHang");

            migrationBuilder.DropColumn(
                name: "DaiLy",
                table: "PhieuXuatHangs");

            migrationBuilder.RenameColumn(
                name: "PhieuXuatHangID",
                table: "ChiTietXuatHang",
                newName: "PhieuXuatHangId");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietXuatHang_PhieuXuatHangID",
                table: "ChiTietXuatHang",
                newName: "IX_ChiTietXuatHang_PhieuXuatHangId");

            migrationBuilder.AddColumn<int>(
                name: "DaiLyId",
                table: "PhieuXuatHangs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "PhieuXuatHangId",
                table: "ChiTietXuatHang",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HangHoaId",
                table: "ChiTietXuatHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SoLuong",
                table: "ChiTietXuatHang",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "HangHoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenHang = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DonViTinh = table.Column<int>(type: "int", nullable: false),
                    SoLuongTrongKho = table.Column<int>(type: "int", nullable: false),
                    DonGia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangHoa", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuatHangs_DaiLyId",
                table: "PhieuXuatHangs",
                column: "DaiLyId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietXuatHang_HangHoaId",
                table: "ChiTietXuatHang",
                column: "HangHoaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietXuatHang_HangHoa_HangHoaId",
                table: "ChiTietXuatHang",
                column: "HangHoaId",
                principalTable: "HangHoa",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietXuatHang_PhieuXuatHangs_PhieuXuatHangId",
                table: "ChiTietXuatHang",
                column: "PhieuXuatHangId",
                principalTable: "PhieuXuatHangs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PhieuXuatHangs_DaiLys_DaiLyId",
                table: "PhieuXuatHangs",
                column: "DaiLyId",
                principalTable: "DaiLys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietXuatHang_HangHoa_HangHoaId",
                table: "ChiTietXuatHang");

            migrationBuilder.DropForeignKey(
                name: "FK_ChiTietXuatHang_PhieuXuatHangs_PhieuXuatHangId",
                table: "ChiTietXuatHang");

            migrationBuilder.DropForeignKey(
                name: "FK_PhieuXuatHangs_DaiLys_DaiLyId",
                table: "PhieuXuatHangs");

            migrationBuilder.DropTable(
                name: "HangHoa");

            migrationBuilder.DropIndex(
                name: "IX_PhieuXuatHangs_DaiLyId",
                table: "PhieuXuatHangs");

            migrationBuilder.DropIndex(
                name: "IX_ChiTietXuatHang_HangHoaId",
                table: "ChiTietXuatHang");

            migrationBuilder.DropColumn(
                name: "DaiLyId",
                table: "PhieuXuatHangs");

            migrationBuilder.DropColumn(
                name: "HangHoaId",
                table: "ChiTietXuatHang");

            migrationBuilder.DropColumn(
                name: "SoLuong",
                table: "ChiTietXuatHang");

            migrationBuilder.RenameColumn(
                name: "PhieuXuatHangId",
                table: "ChiTietXuatHang",
                newName: "PhieuXuatHangID");

            migrationBuilder.RenameIndex(
                name: "IX_ChiTietXuatHang_PhieuXuatHangId",
                table: "ChiTietXuatHang",
                newName: "IX_ChiTietXuatHang_PhieuXuatHangID");

            migrationBuilder.AddColumn<string>(
                name: "DaiLy",
                table: "PhieuXuatHangs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PhieuXuatHangID",
                table: "ChiTietXuatHang",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_ChiTietXuatHang_PhieuXuatHangs_PhieuXuatHangID",
                table: "ChiTietXuatHang",
                column: "PhieuXuatHangID",
                principalTable: "PhieuXuatHangs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
