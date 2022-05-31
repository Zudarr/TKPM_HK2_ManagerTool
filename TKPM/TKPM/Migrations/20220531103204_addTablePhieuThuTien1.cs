using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class addTablePhieuThuTien1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PhieuThuTiens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdNguoiThuTien = table.Column<int>(type: "int", nullable: false),
                    IdDaiLy = table.Column<int>(type: "int", nullable: false),
                    NgayThuTien = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SoTienThu = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhieuThuTiens", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PhieuThuTiens");
        }
    }
}
