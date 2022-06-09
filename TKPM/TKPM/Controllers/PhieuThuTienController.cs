using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TKPM.Data;
using TKPM.Models;
using System.Security.Claims;

namespace TKPM.Controllers
{
    [Authorize]
    public class PhieuThuTienController : Controller
    {
        private string UserID { get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }}
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
            return View("TaoPhieuThuTien");
        }
        [Authorize(Roles = "QuanLyCongTy,QuanLyKho")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PhieuThuTien obj)
        {
            if(obj.SoTienThu==null)
            {
                ViewData["ErrorRaised"] = "Raised";
                ViewData["ErrorMessage"] = "Chưa nhập số tiền thu";
                return RedirectToAction("LapPhieuThuTien");
            }
            obj.IdNguoiThuTien = UserID;
            var daiLyNeedToUpdate = _db.DaiLys.First(c => c.Id == obj.IdDaiLy);
            if (daiLyNeedToUpdate == null)
            {
                ViewData["ErrorRaised"] = "Raised";
                ViewData["ErrorMessage"] = "Không thể tìm được đại lý";
                return RedirectToAction("LapPhieuThuTien");
            }
            if (daiLyNeedToUpdate.NoHienTai-obj.SoTienThu<0)
            {
                ViewData["ErrorRaised"]= "Raised";
                ViewData["ErrorMessage"] = "Số tiền trả lớn hơn số tiền nợ";
                return RedirectToAction("LapPhieuThuTien");
            }
            daiLyNeedToUpdate.NoHienTai -= obj.SoTienThu;
            _db.PhieuThuTiens.Add(obj);
            _db.DaiLys.Update(daiLyNeedToUpdate);
            _db.SaveChanges();
            return RedirectToAction("LichSuThuTien");
        }
        public ActionResult ExportPDF_ChiTietThuTien_BM4(PhieuThuTien obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                //=============Start creating pdf content================
                //Add heading
                Paragraph para1 = new Paragraph($"PHIEU THU TIEN\n\n", new Font(Font.FontFamily.HELVETICA, 20, 1));
                Paragraph para2 = new Paragraph($"Dai ly: {obj.DaiLy.TenDaiLy}\n".NonUnicode());
                Paragraph para3 = new Paragraph($"Dia chi: {obj.DaiLy.DiaChiDaiLy}\n".NonUnicode());
                Paragraph para4 = new Paragraph($"Dien thoai: {obj.DaiLy.DienThoaiDaiLy}\n");

                Paragraph para5 = new Paragraph($"Email: {obj.DaiLy.EmailDaiLy}\n");
                Paragraph para6 = new Paragraph($"Ngay thu tien: {obj.NgayThuTien.ToString()}\n");
                Paragraph para7 = new Paragraph($"So tien thu: {obj.SoTienThu}\n");


                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);
                document.Add(para2);
                document.Add(para3);
                document.Add(para4);
                document.Add(para5);
                document.Add(para6);
                document.Add(para7);

                //=============End creating pdf content================
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "application/vnd", $"PhieuThuTien.pdf");
            }
            return View();
        }
        public IActionResult ChiTietThuTien(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var obj=_db.PhieuThuTiens.Include(e => e.DaiLy).Include(f=>f.ApplicationUser).SingleOrDefault(c=>c.Id==id);
            return View("ChiTietThuTien", obj);
        }
        [Authorize(Roles = "QuanLyCongTy")]
        public IActionResult Delete(int?id)
        {
            var obj = _db.PhieuThuTiens.Find(id);
            var daiLy=_db.DaiLys.First(c => c.Id == obj.IdDaiLy);
            var noSauKhiXoa = daiLy.NoHienTai + obj.SoTienThu;
            var noToiDa = 0;
            if(daiLy.LoaiDaiLy==1)
            {
                var quyDinhDL1 = from danhSachQuyDinh in _db.QuyDinhs
                                  where danhSachQuyDinh.MaNhanDien == "TN_DL1"
                                  select danhSachQuyDinh.GiaTri;
                noToiDa = quyDinhDL1.First();
            }
            else
            {
                var quyDinhDL2 =  from danhSachQuyDinh in _db.QuyDinhs
                                  where danhSachQuyDinh.MaNhanDien == "TN_DL2"
                                  select danhSachQuyDinh.GiaTri;
                noToiDa= quyDinhDL2.First();
            }
            if(noSauKhiXoa>noToiDa)
            {
                return RedirectToAction("ChiTietThuTien", obj.Id);
            }
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
            var noToiDa = 0;
            var daiLy=_db.DaiLys.Find(obj.IdDaiLy);
            if (daiLy.LoaiDaiLy == 1)
            {
                var quyDinhDL1 = from danhSachQuyDinh in _db.QuyDinhs
                                 where danhSachQuyDinh.MaNhanDien == "TN_DL1"
                                 select danhSachQuyDinh.GiaTri;
                noToiDa = quyDinhDL1.First();
            }
            else
            {
                var quyDinhDL2 = from danhSachQuyDinh in _db.QuyDinhs
                                 where danhSachQuyDinh.MaNhanDien == "TN_DL2"
                                 select danhSachQuyDinh.GiaTri;
                noToiDa = quyDinhDL2.First();
            }

            var oldPhieuThuTien=_db.PhieuThuTiens.Find(obj.Id);
            var tienBanDau = daiLy.NoHienTai+oldPhieuThuTien.SoTienThu;
            var tienNoSauUpdate = tienBanDau - obj.SoTienThu;

            if(tienNoSauUpdate<0||tienNoSauUpdate>noToiDa)
            {
                return RedirectToAction("ChiTietThuTien",obj.Id);
            }
            daiLy.NoHienTai = tienNoSauUpdate;

            _db.DaiLys.Update(daiLy);
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
