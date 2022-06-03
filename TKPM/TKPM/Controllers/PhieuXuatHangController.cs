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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(PhieuXuatHang obj)
        {
            var result = _db.Update(new PhieuXuatHang() { DaiLyId = obj.DaiLyId ,ID=obj.ID,NgayLapPhieuXuatHang=obj.NgayLapPhieuXuatHang});
            _db.SaveChanges();

            for (int i = 0; i < obj.ChiTietXuatHangs.Count(); i++)
            {
                _db.Update(obj.ChiTietXuatHangs[i]);
                _db.SaveChanges();
            }
            return RedirectToAction(actionName: "Edit", new { id = obj.ID });
        }
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
    }
}
