using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TKPM.Models
{
    public class BaoCaoCongNo
    {
        public DaiLy DaiLy { get; set; }
        public List<PhieuXuatHang> PhieuXuatHangs { get; set; }
        public List<PhieuThuTien> PhieuThuTiens { get; set; }
        public int NoDau { get; set; }
        public int TongPhatSinh { get; set; }
        public int NoCuoi { get; set; }
    }

    public class BaoCaoCongNoVM
    {
        public int Thang { get; set; }
        public List<BaoCaoCongNo> BaoCaoCongNo { get; set; }
    }
}
