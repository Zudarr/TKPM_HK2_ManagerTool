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
    [Authorize]
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
            return View(quyDinh.ToList());
        }
        [Authorize(Roles = "QuanLyCongTy")]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var quyDinh = _db.QuyDinhs.FirstOrDefault(q => q.ID == id);
            return View("SuaQuyDinh", quyDinh);
        }
        [Authorize(Roles = "QuanLyCongTy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(QuyDinh obj)
        {

            _db.QuyDinhs.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Sort(string sortOrder)
        {
            ViewBag.MaSortParm = string.IsNullOrEmpty(sortOrder) ? "MaDaiLy_desc" : "";
            var quyDinhs = from q in _db.QuyDinhs select q;
            quyDinhs = sortOrder switch
            {
                "MaDaiLy_desc" => quyDinhs.OrderByDescending(q => q.MaNhanDien),
                _ => quyDinhs.OrderBy(q => q.MaNhanDien),
            };
            return View("Index", quyDinhs);
        }
    }
}
