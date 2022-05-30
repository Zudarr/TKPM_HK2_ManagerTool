using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class addPhieuXuatHangToDataBase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuXuatHangs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDaiLy = table.Column<int>(type: "int", nullable: false),
                    MaTaiKhoanLap = table.Column<int>(type: "int", nullable: false),
                    NgayLapPhieuXuatHang = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuXuatHangs", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhieuXuatHangs");
        }
    }
}
