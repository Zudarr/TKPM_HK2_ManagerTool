using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TKPM.Models
{
    public class DaiLy
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string TenDaiLy { get; set; }
        public int LoaiDaiLy { get; set; }
        public string DienThoaiDaiLy { get; set; }
        public string DiaChiDaiLy { get; set; }
        public string QuanDaiLy { get; set; }
        public DateTime NgayTiepNhan { get; set; }
        public string EmailDaiLy { get; set; }
        public int NoDau { get; set; }
        public int PhatSinh { get; set; }
        public int NoCuoi { get; set; }
        public int MaNguoiQuanLy { get; set; }
}
}
