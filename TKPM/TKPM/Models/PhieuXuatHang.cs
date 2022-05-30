using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKPM.Models
{
    public class PhieuXuatHang
    {
        [Key]
        public int ID { get; set; }
        public DaiLy DaiLy { get; set; } 
        public int DaiLyId { get; set; }
        public DateTime NgayLapPhieuXuatHang { get; set; } = DateTime.Now;
        public List<ChiTietXuatHang> ChiTietXuatHangs { get; set; }

    }
}
