using Microsoft.AspNetCore.Mvc;
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
            return View();
        }

        public IActionResult LichSuXuatHang()
        {
            IEnumerable<PhieuXuatHang> PhieuXuatHangList = _db.PhieuXuatHangs;
            return View(PhieuXuatHangList);
        }
    }
}
