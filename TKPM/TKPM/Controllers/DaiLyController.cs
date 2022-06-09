using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TKPM.Data;
using TKPM.Models;
using TKPM.Models.ViewModels;

namespace TKPM.Controllers
{
    [Authorize]
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
        public ActionResult ExportPDF_DanhSachDaiLy_BM3(IEnumerable<DaiLy> obj_temp)
        {
            List<DaiLy> obj = _db.DaiLys.ToList();
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                //=============Start creating pdf content================
                //Add heading
                Paragraph para1 = new Paragraph($"DANH SACH DAI LY\n\n", new Font(Font.FontFamily.HELVETICA, 20, 1));

                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);

                //Add table
                int Col = 5;
                PdfPTable table = new PdfPTable(Col);

                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 30f, 250f, 90f, 90f, 100f };
                table.SetWidths(widths);

                string[] headers = new string[] { "Stt", "Dai ly", "Loai", "Quan", "Tien no" };
                for (int i = 0; i < Col; i++)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(headers[i]));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY;
                    cell.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                    cell.BorderWidthBottom = 1f;
                    cell.BorderWidthTop = 1f;
                    cell.BorderWidthLeft = 1f;
                    cell.BorderWidthRight = 1f;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_CENTER;
                    table.AddCell(cell);
                }

                for (int i = 0; i < obj.Count(); i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(i.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(obj[i].TenDaiLy.NonUnicode()));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(obj[i].LoaiDaiLy.ToString()));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(obj[i].QuanDaiLy.ToString().NonUnicode()));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(obj[i].NoHienTai.ToString()));

                    cell_1.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_2.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_3.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_4.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell_5.HorizontalAlignment = Element.ALIGN_CENTER;

                    table.AddCell(cell_1);
                    table.AddCell(cell_2);
                    table.AddCell(cell_3);
                    table.AddCell(cell_4);
                    table.AddCell(cell_5);
                }

                //=============End creating pdf content================
                document.Add(table);
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "application/vnd", $"DanhSachDaiLy.pdf");
            }
            return View();
        }

        public IActionResult DanhSachDaiLy()
        {
            IQueryable<DaiLy> danhSachDaiLy = _db.DaiLys;
            return View("DanhSachDaiLy", danhSachDaiLy.ToList());
        }
        [Authorize(Roles ="QuanLyCongTy")]
        public IActionResult ThemDaiLy()
        {
            return View();
        }
        [Authorize(Roles = "QuanLyCongTy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(DaiLy obj)
        {
            obj.NgayTiepNhan = System.DateTime.Now;
            //obj.NoDau = 0;
            //obj.NoCuoi = 0;
            //obj.PhatSinh = 0;
            var DaiLyCungQuan = from DaiLy in _db.DaiLys
                               where DaiLy.QuanDaiLy == obj.QuanDaiLy
                               select DaiLy.Id;
            var SoLuongDaiLy = DaiLyCungQuan.Count();

            var quyDinhSoLuong = from QuyDinh in _db.QuyDinhs
                                 where QuyDinh.MaNhanDien == "SL_DL_TDQ"
                                 select QuyDinh.GiaTri;
            var giaTriQuyDinh = quyDinhSoLuong.First();

            if(SoLuongDaiLy==giaTriQuyDinh)
            {
                return RedirectToAction("ThemDaiLy");
            }
            _db.DaiLys.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult ExportPDF_ChiTietDaiLy_BM1(DaiLy obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                //=============Start creating pdf content================
                //Add heading
                Paragraph para1 = new Paragraph($"PHIEU CHI TIET DAI LY\n\n", new Font(Font.FontFamily.HELVETICA, 20, 1));
                Paragraph para2 = new Paragraph($"Ten: {obj.TenDaiLy}\n".NonUnicode());
                Paragraph para3 = new Paragraph($"Loai dai ly: {obj.LoaiDaiLy}\n");
                Paragraph para4 = new Paragraph($"Dien thoai: {obj.DienThoaiDaiLy}\n");
                Paragraph para5 = new Paragraph($"Dia chi: {obj.DiaChiDaiLy}\n".NonUnicode());
                Paragraph para6 = new Paragraph($"Quan: {obj.QuanDaiLy}\n".NonUnicode());
                Paragraph para7 = new Paragraph($"Ngay tiep nhan: {obj.NgayTiepNhan.ToString()}\n");
                Paragraph para8 = new Paragraph($"Email: {obj.EmailDaiLy}\n");

                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);
                document.Add(para2);
                document.Add(para3);
                document.Add(para4);
                document.Add(para5);
                document.Add(para6);
                document.Add(para7);
                document.Add(para8);

                //=============End creating pdf content================
                document.Close();
                writer.Close();
                var constant = ms.ToArray();
                return File(constant, "application/vnd", $"ChiTietDaiLy.pdf");
            }
            return View();
        }

        public IActionResult ChiTietDaiLy(int ?id)
        {
            if(id == null||id==0)
            {
                return NotFound();
            }
            DaiLy daiLyTruyXuat= _db.DaiLys.Include(c=>c.ApplicationUser).FirstOrDefault(x => x.Id == id);
            return View("ChiTietDaiLy",daiLyTruyXuat);
        }
        [Authorize(Roles = "QuanLyCongTy")]
        public IActionResult Update(int ?id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            DaiLy daiLyTruyXuat = _db.DaiLys.FirstOrDefault(x => x.Id == id);
            return View("UpdateDaiLy", daiLyTruyXuat);
        }
        [Authorize(Roles = "QuanLyCongTy")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(DaiLy obj)
        {
            _db.DaiLys.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "QuanLyCongTy")]
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
