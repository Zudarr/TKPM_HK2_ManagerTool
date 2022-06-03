using Microsoft.AspNetCore.Mvc;
using System.Linq;
using TKPM.Data;
using TKPM.Models;

namespace TKPM.Controllers
{
    public class PhieuThuTienController : Controller
    {
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
            var obj = _db.PhieuThuTiens.ToList();
            return View("LichSuThuTien",obj);
        }
        public IActionResult LapPhieuThuTien()
        {
            return View("TaoPhieuThuTien");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhieuThuTien obj)
        {
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
        public IActionResult Delete(int?id)
        {
            var obj = _db.PhieuThuTiens.Find(id);
            _db.PhieuThuTiens.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Update(int?id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var obj=_db.PhieuThuTiens.Find(id);
            return View("UpdatePhieuThuTien",obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(PhieuThuTien obj)
        {
            _db.PhieuThuTiens.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("LichSuThuTien");
        }
    }
}
