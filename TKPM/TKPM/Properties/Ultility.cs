using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace TKPM.Properties
{
    public static class Ultility
    {
        public const string Role_User_QuanLyKho = "QuanLyKho";
        public const string Role_User_ThuTien = "ThuTien";
        public const string Role_User_QuanLyCongTy = "QuanLyCongTy";
        public const string Role_User_QuanLyChiNhanh = "QuanLyChiNhanh";
    }
    public class EmailSender:IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}
