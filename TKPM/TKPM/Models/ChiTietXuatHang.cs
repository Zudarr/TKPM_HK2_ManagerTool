using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKPM.Models
{
    public class ChiTietXuatHang
    {
        public int Id { get; set; }
        public PhieuXuatHang PhieuXuatHang { get; set; }
        public int PhieuXuatHangId { get; set; }
        public HangHoa HangHoa { get; set; }
        public int HangHoaId { get; set; }

        public int SoLuong { get; set; }
    }
}
