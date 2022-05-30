using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TKPM.Models
{
    public class HangHoa
    {
        [Key]
        public int Id { get; set; }
        public string TenHang { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuongTrongKho { get; set; }
        public int DonGia { get; set; }
    }
}
