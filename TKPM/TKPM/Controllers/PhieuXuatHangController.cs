using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKPM.Data;
using TKPM.Models;

namespace TKPM.Controllers
{
    [Authorize]
    public class PhieuXuatHangController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PhieuXuatHangController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            IEnumerable<PhieuXuatHang> PhieuXuatHangList = _db.PhieuXuatHangs.Include(c => c.DaiLy);
            return View("LichSuXuatHang", PhieuXuatHangList);
        }
        [Authorize(Roles = "QuanLyKho,QuanLyCongTy,QuanLyChiNhanh")]
        public IActionResult LichSuXuatHang()
        {
            IEnumerable<PhieuXuatHang> PhieuXuatHangList = _db.PhieuXuatHangs.Include(c => c.DaiLy);
            return View(PhieuXuatHangList);
        }
        [Authorize(Roles="QuanLyKho,QuanLyCongTy")]
        public IActionResult Create()
        {
            IEnumerable<HangHoa> hangHoa= _db.HangHoas;
            List<ChiTietXuatHang> chiTietXuatHang=new List<ChiTietXuatHang>();

            for(int i = 0; i < hangHoa.Count(); i++)
            {
                chiTietXuatHang.Add(new ChiTietXuatHang() { HangHoa = hangHoa.ElementAt(i),HangHoaId=hangHoa.ElementAt(i).Id});
            }

            PhieuXuatHang phieuXuatHang = new PhieuXuatHang() {ChiTietXuatHangs=chiTietXuatHang};
            return View(phieuXuatHang);
        }
        [Authorize(Roles="QuanLyKho,QuanLyCongTy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhieuXuatHang obj)
        {
            //Tính lại tổng tiền của phiếu
            int TongTriGia = 0;
            foreach (ChiTietXuatHang chiTietXuatHang in obj.ChiTietXuatHangs)
            {
                TongTriGia += chiTietXuatHang.SoLuong * chiTietXuatHang.HangHoa.DonGia;
            }

            var result=_db.Add(new PhieuXuatHang() { DaiLyId = obj.DaiLyId ,TongTriGia=TongTriGia});
            _db.SaveChanges();

            for (int i = 0; i < obj.ChiTietXuatHangs.Count(); i++)
            {
                ChiTietXuatHang chiTietXuatHang = new ChiTietXuatHang() { HangHoaId = obj.ChiTietXuatHangs[i].HangHoaId, SoLuong = obj.ChiTietXuatHangs[i].SoLuong,PhieuXuatHangId=result.Entity.ID};
                _db.Add(chiTietXuatHang);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");

        }

        [Authorize(Roles = "QuanLyKho,QuanLyCongTy")]
        public IActionResult Edit(int id)
        {
            if (id == null)
                return NotFound();
            IEnumerable<ChiTietXuatHang> obj = _db.ChiTietXuatHangs.Include(c => c.HangHoa).Where(c => c.PhieuXuatHangId == id);
            var phieuXuatHang= _db.PhieuXuatHangs.Find(id);
            phieuXuatHang.ChiTietXuatHangs = obj.ToList();

            if (obj == null)
                return NotFound();

            return View(phieuXuatHang);
        }

        [Authorize(Roles = "QuanLyKho,QuanLyCongTy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PhieuXuatHang obj)
        {
            //Tính lại tổng tiền của phiếu
            int TongTriGia = 0;
            foreach (ChiTietXuatHang chiTietXuatHang in obj.ChiTietXuatHangs)
            {
                TongTriGia += chiTietXuatHang.SoLuong * chiTietXuatHang.HangHoa.DonGia;
            }

            var result = _db.Update(new PhieuXuatHang() { DaiLyId = obj.DaiLyId ,ID=obj.ID,NgayLapPhieuXuatHang=obj.NgayLapPhieuXuatHang,TongTriGia= TongTriGia});
            _db.SaveChanges();

            for (int i = 0; i < obj.ChiTietXuatHangs.Count(); i++)
            {
                _db.Update(obj.ChiTietXuatHangs[i]);
                _db.SaveChanges();
            }
            return RedirectToAction(actionName: "Edit", new { id = obj.ID });
        }
        [Authorize(Roles = "QuanLyKho,QuanLyCongTy")]
        public IActionResult Delete(int Id)
        {
            var obj = _db.PhieuXuatHangs.Where(c=>c.ID==Id).Include(c=>c.ChiTietXuatHangs).ToList()[0];

            for (int i = 0; i < obj.ChiTietXuatHangs.Count(); i++)
            {
                _db.Remove(obj.ChiTietXuatHangs[i]);
                _db.SaveChanges();
            }

            _db.Remove(obj);
            _db.SaveChanges();


            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "QuanLyKho,QuanLyCongTy")]
        public IActionResult Delete(PhieuXuatHang obj)
        {
            var result = _db.Remove(new PhieuXuatHang() { DaiLyId = obj.DaiLyId, ID = obj.ID, NgayLapPhieuXuatHang = obj.NgayLapPhieuXuatHang });

            _db.SaveChanges();

            for (int i = 0; i < obj.ChiTietXuatHangs.Count(); i++)
            {
                _db.Remove(obj.ChiTietXuatHangs[i]);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Sort(string sortOrder)
        {
            ViewBag.MaPhieuSortParm = string.IsNullOrEmpty(sortOrder) ? "MaPhieu_desc" : "";
            ViewBag.MaDaiLySortParm = sortOrder == "MaDaiLy" ? "MaDaiLy_desc" : "MaDaiLy";
            ViewBag.NgaySortParm = sortOrder == "NgayLapPhieu" ? "NgayLapPhieu_desc" : "NgayLapPhieu";
            ViewBag.TienSortParm = sortOrder == "TongTien" ? "TongTien_desc" : "TongTien";
            var phieuXuatHangs = from p in _db.PhieuXuatHangs select p;
            phieuXuatHangs = sortOrder switch
            {
                "MaPhieu_desc" => phieuXuatHangs.OrderByDescending(p => p.ID),
                "MaDaiLy_desc" => phieuXuatHangs.OrderByDescending(p => p.DaiLyId),
                "MaDaiLy" => phieuXuatHangs.OrderBy(p => p.DaiLyId),
                "NgayLapPhieu_desc" => phieuXuatHangs.OrderByDescending(p => p.NgayLapPhieuXuatHang),
                "NgayLapPhieu" => phieuXuatHangs.OrderBy(p => p.NgayLapPhieuXuatHang),
                "TongTien_desc" => phieuXuatHangs.OrderBy(p => p.TongTriGia),
                "TongTien" => phieuXuatHangs.OrderBy(p => p.TongTriGia),
                _ => phieuXuatHangs.OrderBy(d => d.ID),
            };
            return View("LichSuXuatHang", phieuXuatHangs);
        }
    }
}
