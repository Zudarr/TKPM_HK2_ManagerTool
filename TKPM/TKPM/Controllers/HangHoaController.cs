using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TKPM.Data;
using TKPM.Models;

namespace TKPM.Controllers
{
    [Authorize(Roles = "QuanLyCongTy,QuanLyKho")]
    public class HangHoaController : Controller
    {
        private readonly ApplicationDbContext _db;

        public HangHoaController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var hangHoas = _db.HangHoas;
            if (hangHoas == null)
            {
                return NotFound();
            }
            return View(hangHoas.ToList());
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var hangHoas = _db.HangHoas.FirstOrDefault(h => h.Id == id);
            return View("SuaHangHoa", hangHoas);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(HangHoa obj)
        {
            _db.HangHoas.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View("ThemHangHoa");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HangHoa obj)
        {
            var soLuongMatHang = _db.HangHoas.Count();
            var soLuongDonViTinh = _db.HangHoas
                .GroupBy(h => h.DonViTinh)
                .Count();
            var maxMH = _db.QuyDinhs.FirstOrDefault(sl => sl.MaNhanDien == "SL_MH").GiaTri;
            var maxDVT = _db.QuyDinhs.FirstOrDefault(sl => sl.MaNhanDien == "SL_DVT").GiaTri;
            if (soLuongDonViTinh > maxDVT || soLuongMatHang > maxMH)
            {
                return RedirectToAction("Index");
            }
            _db.HangHoas.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Sort(string sortOrder)
        {
            ViewBag.MaSortParm = string.IsNullOrEmpty(sortOrder) ? "MaDaiLy_desc" : "";
            ViewBag.TenSortParm = sortOrder == "TenMatHang" ? "TenMatHang_desc" : "TenMatHang";
            ViewBag.SoLuongSortParm = sortOrder == "SoLuong" ? "SoLuong_desc" : "SoLuong";
            ViewBag.DonGiaSortParm = sortOrder == "DonGia" ? "DonGia_desc" : "DonGia";
            var hangHoas = from h in _db.HangHoas select h;
            hangHoas = sortOrder switch
            {
                "MaDaiLy_desc" => hangHoas.OrderByDescending(d => d.Id),
                "TenMatHang_desc" => hangHoas.OrderByDescending(d => d.TenHang),
                "TenMatHang" => hangHoas.OrderBy(d => d.TenHang),
                "SoLuong_desc" => hangHoas.OrderByDescending(d => d.SoLuongTrongKho),
                "SoLuong" => hangHoas.OrderBy(d => d.SoLuongTrongKho),
                "DonGia_desc" => hangHoas.OrderByDescending(d => d.DonGia),
                "DonGia" => hangHoas.OrderBy(d => d.DonGia),
                _ => hangHoas.OrderBy(d => d.Id),
            };
            return View("Index", hangHoas);
        }
    }
}
