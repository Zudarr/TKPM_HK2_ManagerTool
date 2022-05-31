using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TKPM.Models
{
    public class TaiKhoan
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TenTaiKhoan { get; set; }
        public string MatKhauTaiKhoan { get; set; }
        public string HoVaTen { get; set; }
        public int LoaiTaiKhoan { get; set; }

    }
}
