using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TKPM.Models
{
    public class PhieuXuatHang
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public DaiLy DaiLy { get; set; } 
        public int DaiLyId { get; set; }
        public DateTime NgayLapPhieuXuatHang { get; set; } = DateTime.Now;
        public List<ChiTietXuatHang> ChiTietXuatHangs { get; set; }
        public int TongTriGia { get; set; }

    }
}
