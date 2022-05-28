using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TKPM.Data;
using TKPM.Models;

namespace TKPM.Controllers
{
    public class DaiLyController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DaiLyController(ApplicationDbContext db)
        {
            _db= db;
        }
        public IActionResult Index()
        {
            var danhSachDaiLy = _db.DaiLys.ToList();
            return RedirectToAction("DanhSachDaiLy");
        }
        public IActionResult DanhSachDaiLy()
        {
            IEnumerable<DaiLy> danhSachDaiLy = _db.DaiLys.ToList();
            return View("DanhSachDaiLy", danhSachDaiLy);
        }
        public IActionResult ThemDaiLy()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DaiLy obj)
        {
            obj.NgayTiepNhan = System.DateTime.Now;
            obj.NoDau = 0;
            obj.NoCuoi = 0;
            obj.PhatSinh = 0;
            _db.DaiLys.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ChiTietDaiLy()
        {
            return View();
        }
        public IActionResult TraCuuDaiLy()
        {
            return View("TraCuuDaiLy",null);
        }
    }
}
