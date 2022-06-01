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

        public IActionResult LichSuXuatHang()
        {
            IEnumerable<PhieuXuatHang> PhieuXuatHangList = _db.PhieuXuatHangs.Include(c => c.DaiLy);
            return View(PhieuXuatHangList);
        }
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhieuXuatHang obj)
        {
            var result=_db.Add(new PhieuXuatHang() { DaiLyId = obj.DaiLyId });
            _db.SaveChanges();

            System.Diagnostics.Debug.WriteLine(result);

            for (int i = 0; i < obj.ChiTietXuatHangs.Count(); i++)
            {
                ChiTietXuatHang chiTietXuatHang = new ChiTietXuatHang() { HangHoaId = obj.ChiTietXuatHangs[i].HangHoaId, SoLuong = obj.ChiTietXuatHangs[i].SoLuong,PhieuXuatHangId=result.Entity.ID};
                _db.Add(chiTietXuatHang);
                _db.SaveChanges();
            }
            return RedirectToAction("Index");

        }
    }
}
