using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TKPM.Models
{
    public class QuyDinh
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public string MaNhanDien { get; set; }
        public string NoiDung { get; set; }
        public int GiaTri { get; set; }
        public bool Changeable { get; set; }
    }
}