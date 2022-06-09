using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TKPM.Models
{
    public class PhieuThuTien
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public string IdNguoiThuTien{ get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        [ForeignKey("DaiLy")]
        public int IdDaiLy { get; set; }
        public DaiLy DaiLy { get; set; }
        public DateTime NgayThuTien { get; set; } = DateTime.Now;
        public int SoTienThu { get; set; }
    }
}
