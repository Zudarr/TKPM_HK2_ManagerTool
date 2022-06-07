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
        public int IdNguoiThuTien{ get; set; }
        public int IdDaiLy { get; set; }
        public DateTime NgayThuTien { get; set; } = DateTime.Now;
        public int SoTienThu { get; set; }
    }
}
