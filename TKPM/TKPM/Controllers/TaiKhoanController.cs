using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TKPM.Data;
using TKPM.Models;
namespace TKPM.Controllers
{
    public class TaiKhoanController : Controller
    {
        private readonly ApplicationDbContext _db;
        public TaiKhoanController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return RedirectToAction("DanhSachTaiKhoan");
        }
        public IActionResult DanhSachTaiKhoan()
        {
            IQueryable<ApplicationUser> danhSachTaiKhoan = _db.TaiKhoans;
            return View("DanhSachTaiKhoan", danhSachTaiKhoan.ToList());
        }
        public IActionResult XoaTaiKhoan(string? id)
        {
            ApplicationUser TaiKhoanXoa = _db.TaiKhoans.FirstOrDefault(x => x.Id == id);
            _db.TaiKhoans.Remove(TaiKhoanXoa);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
