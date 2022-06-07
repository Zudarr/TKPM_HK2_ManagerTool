using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKPM.Models
{
    public class BaoCaoDoanhThu
    {
        public DaiLy DaiLy { get; set; }
        public List<PhieuXuatHang> PhieuXuatHangs { get; set; }
        public int TongTriGia { get; set; }
        public float TiLe { get; set; }

    }

    public class BaoCaoDoanhThuVM
    {
        public int Thang { get; set; }
        public List<BaoCaoDoanhThu> BaoCaoDoanhThu { get; set; }
    }
}
