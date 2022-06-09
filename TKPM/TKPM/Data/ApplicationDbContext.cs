using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TKPM.Models;
namespace TKPM.Data
{
    public class ApplicationDbContext: IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<DaiLy>()
                .HasIndex(u => u.TenDaiLy)
                .IsUnique(true);
            base.OnModelCreating(builder);
        }
        public DbSet<DaiLy> DaiLys { get; set; }
        public DbSet<PhieuXuatHang> PhieuXuatHangs { get; set; }
        public DbSet<ChiTietXuatHang> ChiTietXuatHangs { get; set; }
        public DbSet<HangHoa> HangHoas { get; set; }
        public DbSet<ApplicationUser> TaiKhoans { get; set; }
        public DbSet<PhieuThuTien> PhieuThuTiens { get; set; }
        public DbSet<QuyDinh> QuyDinhs { get; set; }

    }
}
