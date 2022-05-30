using Microsoft.EntityFrameworkCore.Migrations;
using System;

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class reAddPhieuXuatHangToDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuXuatHangs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DaiLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayLapPhieuXuatHang = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuXuatHangs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ChiTietXuatHang",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PhieuXuatHangID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChiTietXuatHang", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChiTietXuatHang_PhieuXuatHangs_PhieuXuatHangID",
                        column: x => x.PhieuXuatHangID,
                        principalTable: "PhieuXuatHangs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChiTietXuatHang_PhieuXuatHangID",
                table: "ChiTietXuatHang",
                column: "PhieuXuatHangID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChiTietXuatHang");

            migrationBuilder.DropTable(
                name: "PhieuXuatHangs");
        }
    }
}
