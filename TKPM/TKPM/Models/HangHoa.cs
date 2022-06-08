using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TKPM.Models
{
    public class HangHoa
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TenHang { get; set; }
        public string DonViTinh { get; set; }
        public int SoLuongTrongKho { get; set; }
        public int DonGia { get; set; }
    }
}
