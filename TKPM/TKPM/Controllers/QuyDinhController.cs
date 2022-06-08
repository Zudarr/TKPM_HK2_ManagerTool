using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TKPM.Data;
using TKPM.Models;

namespace TKPM.Controllers
{
    public class QuyDinhController : Controller
    {
        private readonly ApplicationDbContext _db;

        public QuyDinhController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var quyDinh = _db.QuyDinhs;
            if (quyDinh == null)
            {
                return NotFound();
            }
            return View(_db.QuyDinhs.ToList());
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var quyDinh = _db.QuyDinhs.FirstOrDefault(q => q.ID == id);
            return View("SuaQuyDinh", quyDinh);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuyDinh obj)
        {

            _db.QuyDinhs.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Sort(string sortOrder, int type)
        {
            ViewBag.MaSortParm = string.IsNullOrEmpty(sortOrder) ? "MaDaiLy_desc" : "";
            ViewBag.TenSortParm = sortOrder == "TenDaiLy" ? "TenDaily_desc" : "TenDaiLy";
            ViewBag.QuanSortParm = sortOrder == "QuanDaiLy" ? "QuanDaiLy_desc" : "QuanDaiLy";
            ViewBag.NgaySortParm = sortOrder == "NgayDaiLy" ? "NgayDaiLy_desc" : "NgayDaiLy";
            var daiLys = from d in _db.DaiLys select d;
            daiLys = sortOrder switch
            {
                "MaDaiLy_desc" => daiLys.OrderByDescending(d => d.Id),
                "TenDaily_desc" => daiLys.OrderByDescending(d => d.TenDaiLy),
                "TenDaiLy" => daiLys.OrderBy(d => d.TenDaiLy),
                "QuanDaiLy_desc" => daiLys.OrderByDescending(d => d.QuanDaiLy),
                "QuanDaiLy" => daiLys.OrderBy(d => d.QuanDaiLy),
                "NgayDaiLy_desc" => daiLys.OrderByDescending(d => d.NgayTiepNhan),
                "NgayDaiLy" => daiLys.OrderBy(d => d.NgayTiepNhan),
                _ => daiLys.OrderBy(d => d.Id),
            };
            return type switch
            {
                1 => View("DanhSachDaiLy", daiLys),
                2 => View("TraCuuDaiLy", daiLys),
                _ => View("DanhSachDaiLy", daiLys),
            };
        }
    }
}
