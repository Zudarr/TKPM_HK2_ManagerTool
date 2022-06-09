using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TKPM.Data;
using TKPM.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace TKPM.Controllers
{
    [Authorize]
    public class PhieuThuTienController : Controller
    {
        private string UserID { get; set; }
        private readonly ApplicationDbContext _db;
        public PhieuThuTienController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return RedirectToAction("LichSuThuTien");
        }
        public IActionResult LichSuThuTien()
        {
            var obj = _db.PhieuThuTiens.Include(c=>c.DaiLy).Include(d=>d.ApplicationUser).ToList();
            return View("LichSuThuTien",obj);
        }
        [Authorize(Roles = "QuanLyCongTy,QuanLyKho")]
        public IActionResult LapPhieuThuTien()
        {
            ViewData["DaiLy"]= _db.DaiLys.ToList();
            UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View("TaoPhieuThuTien");
        }
        [Authorize(Roles = "QuanLyCongTy,QuanLyKho")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhieuThuTien obj)
        {
            obj.IdNguoiThuTien = UserID;
            if(obj.DaiLy.NoHienTai-obj.SoTienThu<0)
            {
                return RedirectToAction("Create");
            }
            _db.PhieuThuTiens.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("LichSuThuTien");
        }

        public IActionResult ChiTietThuTien(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var obj=_db.PhieuThuTiens.Find(id);
            return View("ChiTietThuTien", obj);
        }
        [Authorize(Roles = "QuanLyCongTy")]
        public IActionResult Delete(int?id)
        {
            var obj = _db.PhieuThuTiens.Find(id);
            _db.PhieuThuTiens.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "QuanLyCongTy")]
        public IActionResult Update(int?id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var obj=_db.PhieuThuTiens.Find(id);
            return View("UpdatePhieuThuTien",obj);
        }
        [Authorize(Roles = "QuanLyCongTy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(PhieuThuTien obj)
        {
            _db.PhieuThuTiens.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("LichSuThuTien");
        }

        public IActionResult Sort(string sortOrder)
        {
            ViewBag.MaPhieuSortParm = string.IsNullOrEmpty(sortOrder) ? "MaPhieu_desc" : "";
            ViewBag.NguoiSortParm = sortOrder == "NguoiThuTien" ? "NguoiThuTien_desc" : "NguoiThuTien";
            ViewBag.NgaySortParm = sortOrder == "NgayThuTien" ? "NgayThuTien_desc" : "NgayThuTien";
            ViewBag.TienSortParm = sortOrder == "SoTienThu" ? "SoTienThu_desc" : "SoTienThu";
            var phieuThuTiens = from p in _db.PhieuThuTiens select p;
            phieuThuTiens = sortOrder switch
            {
                "MaPhieu_desc" => phieuThuTiens.OrderByDescending(p => p.Id),
                "NguoiThuTien_desc" => phieuThuTiens.OrderByDescending(p => p.IdNguoiThuTien),
                "NguoiThuTien" => phieuThuTiens.OrderBy(p => p.IdNguoiThuTien),
                "NgayThuTien_desc" => phieuThuTiens.OrderByDescending(p => p.NgayThuTien),
                "NgayThuTien" => phieuThuTiens.OrderBy(p => p.NgayThuTien),
                "SoTienThu_desc" => phieuThuTiens.OrderBy(p => p.SoTienThu),
                "SoTienThu" => phieuThuTiens.OrderBy(p => p.SoTienThu),
                _ => phieuThuTiens.OrderBy(d => d.Id),
            };
            return View("LichSuThuTien", phieuThuTiens);
        }
    }
}
