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
            return RedirectToAction("DanhSachDaiLy");
        }
        public IActionResult DanhSachDaiLy()
        {
            IQueryable<DaiLy> danhSachDaiLy = _db.DaiLys;
            return View("DanhSachDaiLy", danhSachDaiLy.ToList());
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
            //obj.NoDau = 0;
            //obj.NoCuoi = 0;
            //obj.PhatSinh = 0;

            _db.DaiLys.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ChiTietDaiLy(int ?id)
        {
            if(id == null||id==0)
            {
                return NotFound();
            }
            DaiLy daiLyTruyXuat= _db.DaiLys.FirstOrDefault(x => x.Id == id);
            return View("ChiTietDaiLy",daiLyTruyXuat);
        }

        public IActionResult Update(int ?id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            DaiLy daiLyTruyXuat = _db.DaiLys.FirstOrDefault(x => x.Id == id);
            return View("UpdateDaiLy", daiLyTruyXuat);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DaiLy obj)
        {
            _db.DaiLys.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult Delete(int? id)
        {
            DaiLy daiLyXoa = _db.DaiLys.FirstOrDefault(x => x.Id == id);
            _db.DaiLys.Remove(daiLyXoa);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult TraCuuDaiLy(string name, string district)
        {
            var daiLys = from d in _db.DaiLys select d;
            if (!string.IsNullOrEmpty(name))
            {
                daiLys = daiLys.Where(d => d.TenDaiLy.Contains(name));
            }
            if (!string.IsNullOrEmpty(district))
            {
                daiLys = daiLys.Where(d => d.QuanDaiLy == district);
            }
            return View(daiLys);
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
