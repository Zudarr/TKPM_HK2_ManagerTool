using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TKPM.Migrations
{
    public partial class initDatabaseAndCreateDaiLyTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DaiLys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenDaiLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoaiDaiLy = table.Column<int>(type: "int", nullable: false),
                    DienThoaiDaiLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiaChiDaiLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    QuanDaiLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NgayTiepNhan = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmailDaiLy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoDau = table.Column<int>(type: "int", nullable: false),
                    PhatSinh = table.Column<int>(type: "int", nullable: false),
                    NoCuoi = table.Column<int>(type: "int", nullable: false),
                    MaNguoiQuanLy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DaiLys", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DaiLys");
        }
    }
}
