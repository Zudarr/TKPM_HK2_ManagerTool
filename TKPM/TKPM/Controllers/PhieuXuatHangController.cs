using iTextSharp.text;
using iTextSharp.text.pdf;
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
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
                //_db.SaveChanges();

            }

            //Cập nhật nợ cho đại lý
            DaiLy daiLy = _db.DaiLys.FirstOrDefault(c => c.Id == obj.DaiLyId);
            daiLy.NoHienTai += TongTriGia;

            //Nợ quá quy định thì không được tao
            if((daiLy.LoaiDaiLy == 1 && daiLy.NoHienTai > 2000000) || (daiLy.LoaiDaiLy == 2 && daiLy.NoHienTai > 5000000))
                return RedirectToAction("Index");
            _db.Update(daiLy);
            _db.SaveChanges();
            return RedirectToAction("Index");

        }
        public ActionResult ExportPDF_ChiTietXuatHang_BM2(PhieuXuatHang obj)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                //=============Start creating pdf content================
                //Add heading
                Paragraph para1 = new Paragraph($"PHIEU XUAT HANG\n\n", new Font(Font.FontFamily.HELVETICA, 20,1));
                Paragraph para2 = new Paragraph($"       Dai ly: {obj.DaiLy.TenDaiLy}                  Ngay lap phieu: {obj.NgayLapPhieuXuatHang.ToShortDateString()}\n\n".NonUnicode());

                para1.Alignment= Element.ALIGN_CENTER;
                document.Add(para1);
                document.Add(para2);


                //Add table
                PdfPTable table = new PdfPTable(5);

                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 30f, 250f, 90f, 90f, 100f };
                table.SetWidths(widths);

                PdfPCell cell1 = new PdfPCell(new Phrase("Stt"));
                cell1.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell1.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell1.BorderWidthBottom = 1f;
                cell1.BorderWidthTop = 1f;
                cell1.BorderWidthLeft = 1f;
                cell1.BorderWidthRight = 1f;
                cell1.HorizontalAlignment = Element.ALIGN_CENTER;
                cell1.VerticalAlignment = Element.ALIGN_CENTER;


                PdfPCell cell2 = new PdfPCell(new Phrase("Mat hang"));
                cell2.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell2.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell2.BorderWidthBottom = 1f;
                cell2.BorderWidthTop = 1f;
                cell2.BorderWidthLeft = 1f;
                cell2.BorderWidthRight = 1f;
                cell2.HorizontalAlignment = Element.ALIGN_CENTER;
                cell2.VerticalAlignment = Element.ALIGN_CENTER;


                PdfPCell cell3 = new PdfPCell(new Phrase("Don vi tinh"));
                cell3.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell3.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell3.BorderWidthBottom = 1f;
                cell3.BorderWidthTop = 1f;
                cell3.BorderWidthLeft = 1f;
                cell3.BorderWidthRight = 1f;
                cell3.HorizontalAlignment = Element.ALIGN_CENTER;
                cell3.VerticalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell4 = new PdfPCell(new Phrase("So luong"));
                cell4.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell4.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell4.BorderWidthBottom = 1f;
                cell4.BorderWidthTop = 1f;
                cell4.BorderWidthLeft = 1f;
                cell4.BorderWidthRight = 1f;
                cell4.HorizontalAlignment = Element.ALIGN_CENTER;
                cell4.VerticalAlignment = Element.ALIGN_CENTER;

                PdfPCell cell5 = new PdfPCell(new Phrase("Don gia"));
                cell5.BackgroundColor = BaseColor.LIGHT_GRAY;
                cell5.Border = Rectangle.BOTTOM_BORDER | Rectangle.TOP_BORDER | Rectangle.LEFT_BORDER | Rectangle.RIGHT_BORDER;
                cell5.BorderWidthBottom = 1f;
                cell5.BorderWidthTop = 1f;
                cell5.BorderWidthLeft = 1f;
                cell5.BorderWidthRight = 1f;
                cell5.HorizontalAlignment = Element.ALIGN_CENTER;
                cell5.VerticalAlignment = Element.ALIGN_CENTER;

                table.AddCell(cell1);
                table.AddCell(cell2);
                table.AddCell(cell3);
                table.AddCell(cell4);
                table.AddCell(cell5);

                for (int i = 0; i < obj.ChiTietXuatHangs.Count(); i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(i.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(obj.ChiTietXuatHangs[i].HangHoa.TenHang.NonUnicode()));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(obj.ChiTietXuatHangs[i].HangHoa.DonViTinh.NonUnicode()));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(obj.ChiTietXuatHangs[i].SoLuong.ToString()));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(obj.ChiTietXuatHangs[i].HangHoa.DonGia.ToString()));

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
                return File(constant, "application/vnd", "PhieuXuatHang.pdf");
            }
            return View();
        }
        [Authorize(Roles = "QuanLyKho,QuanLyCongTy")]
        public IActionResult Edit(int id)
        {
            if (id == null)
                return NotFound();
            IEnumerable<ChiTietXuatHang> obj = _db.ChiTietXuatHangs.Include(c => c.HangHoa).Where(c => c.PhieuXuatHangId == id);
            var phieuXuatHang= _db.PhieuXuatHangs.Include(c=>c.DaiLy).FirstOrDefault(c=>c.ID==id);
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
