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
        public string DaiLy { get; set; }
        public DateTime NgayLapPhieuXuatHang { get; set; }
        public List<ChiTietXuatHang> ChiTietXuatHangs { get; set; }
    }
}
