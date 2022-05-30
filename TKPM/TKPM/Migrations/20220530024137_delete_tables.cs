using Microsoft.EntityFrameworkCore.Migrations;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class delete_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietXuatHang");

            migrationBuilder.DropTable(
                name: "HangHoa");

            migrationBuilder.DropTable(
                name: "PhieuXuatHangs");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HangHoa",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DonGia = table.Column<int>(type: "int", nullable: false),
                    DonViTinh = table.Column<int>(type: "int", nullable: false),
                    SoLuongTrongKho = table.Column<int>(type: "int", nullable: false),
                    TenHang = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HangHoa", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PhieuXuatHangs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaiLyId = table.Column<int>(type: "int", nullable: false),
                    NgayLapPhieuXuatHang = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuXuatHangs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PhieuXuatHangs_DaiLys_DaiLyId",
                        column: x => x.DaiLyId,
                        principalTable: "DaiLys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietXuatHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HangHoaId = table.Column<int>(type: "int", nullable: false),
                    PhieuXuatHangId = table.Column<int>(type: "int", nullable: false),
                    SoLuong = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietXuatHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietXuatHang_HangHoa_HangHoaId",
                        column: x => x.HangHoaId,
                        principalTable: "HangHoa",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ChiTietXuatHang_PhieuXuatHangs_PhieuXuatHangId",
                        column: x => x.PhieuXuatHangId,
                        principalTable: "PhieuXuatHangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietXuatHang_HangHoaId",
                table: "ChiTietXuatHang",
                column: "HangHoaId");

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietXuatHang_PhieuXuatHangId",
                table: "ChiTietXuatHang",
                column: "PhieuXuatHangId");

            migrationBuilder.CreateIndex(
                name: "IX_PhieuXuatHangs_DaiLyId",
                table: "PhieuXuatHangs",
                column: "DaiLyId");
        }
    }
}
