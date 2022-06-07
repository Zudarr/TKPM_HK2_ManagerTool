using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TKPM.Models
{
    public class ApplicationUser: IdentityUser
    {
        [Required]
        public string TenTaiKhoan { get; set; }
        public string HoVaTen { get; set; }
    }
}
