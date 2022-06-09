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
using TKPM.Models.ViewModels;

namespace TKPM.Controllers
{
    [Authorize]
    public class BaoCaoController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BaoCaoController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public ActionResult ExportPDF_BaoCaoDoanhSo_BM5_1(BaoCaoDoanhThuVM obj_temp)
        {
            BaoCaoDoanhThuVM obj = GetBaoCaoDoanhThuVM(obj_temp.Thang);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                //=============Start creating pdf content================
                //Add heading
                Paragraph para1 = new Paragraph($"PHIEU BAO CAO DOANH SO\n\n", new Font(Font.FontFamily.HELVETICA, 20, 1));
                Paragraph para2 = new Paragraph($"       Thang: {obj.Thang}\n\n".NonUnicode());

                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);
                document.Add(para2);


                //Add table
                int Col = 5;
                PdfPTable table = new PdfPTable(Col);

                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 30f, 250f, 90f, 90f, 100f };
                table.SetWidths(widths);

                string[] headers = new string[] { "Stt","Dai ly","So phieu xuat","Tong tri gia","Ty le"};
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

                for (int i = 0; i < obj.BaoCaoDoanhThu.Count(); i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(i.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(obj.BaoCaoDoanhThu[i].DaiLy.TenDaiLy.NonUnicode()));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(obj.BaoCaoDoanhThu[i].PhieuXuatHangs.Count().ToString()));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(obj.BaoCaoDoanhThu[i].TongTriGia.ToString()));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(obj.BaoCaoDoanhThu[i].TiLe.ToString()));

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
                return File(constant, "application/vnd", $"BaoCaoDoanhThu_{obj.Thang}.pdf");
            }
            return View();
        }
        public IActionResult BaoCaoDoanhThu()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BaoCaoDoanhThu(BaoCaoDoanhThuVM obj)
        {

            BaoCaoDoanhThuVM baoCaoDoanhThuVM = new BaoCaoDoanhThuVM();
            baoCaoDoanhThuVM.BaoCaoDoanhThu = new List<BaoCaoDoanhThu>();

            baoCaoDoanhThuVM.Thang = obj.Thang;

            List<DaiLy> daiLys = _db.DaiLys.ToList();
            for(int i = 0; i < daiLys.Count(); i++)
            {
                BaoCaoDoanhThu baoCaoDoanhThu = new BaoCaoDoanhThu() { DaiLy = daiLys[i], PhieuXuatHangs = _db.PhieuXuatHangs.Where(c => (c.DaiLyId == daiLys[i].Id)&& (c.NgayLapPhieuXuatHang.Month==baoCaoDoanhThuVM.Thang)).ToList()};
                baoCaoDoanhThuVM.BaoCaoDoanhThu.Add(baoCaoDoanhThu);
            }

            //Tính toán tỉ lệ doanh thu
            int TongDoanhThu = 0;

            for (int i = 0; i < baoCaoDoanhThuVM.BaoCaoDoanhThu.Count(); i++)
            {
                for (int j = 0; j < baoCaoDoanhThuVM.BaoCaoDoanhThu[i].PhieuXuatHangs.Count(); j++)
                    //Tính tổng trị giá phiếu xuất hàng của một đại lý
                    baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TongTriGia += baoCaoDoanhThuVM.BaoCaoDoanhThu[i].PhieuXuatHangs[j].TongTriGia;
                //Tính tổng trị giá phiếu xuất hàng của tất cả đại lý
                TongDoanhThu += baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TongTriGia;
            }


            for (int i = 0; i < baoCaoDoanhThuVM.BaoCaoDoanhThu.Count(); i++)
                baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TiLe=((float)baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TongTriGia/TongDoanhThu)*100;

            return View(baoCaoDoanhThuVM);
        }


        public ActionResult ExportPDF_BaoCaoCongNo_BM5_2(BaoCaoCongNoVM obj_temp)
        {
            BaoCaoCongNoVM obj = GetBaoCaoCongNoVM(obj_temp.Thang);

            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);
                document.Open();
                //=============Start creating pdf content================
                //Add heading
                Paragraph para1 = new Paragraph($"PHIEU BAO CAO CONG NO\n\n", new Font(Font.FontFamily.HELVETICA, 20, 1));
                Paragraph para2 = new Paragraph($"       Thang: {obj.Thang}\n\n".NonUnicode());

                para1.Alignment = Element.ALIGN_CENTER;
                document.Add(para1);
                document.Add(para2);


                //Add table
                int Col = 5;
                PdfPTable table = new PdfPTable(Col);

                table.TotalWidth = 500f;
                table.LockedWidth = true;
                float[] widths = new float[] { 30f, 250f, 90f, 90f, 100f };
                table.SetWidths(widths);

                string[] headers = new string[] { "Stt", "Dai ly", "No dau", "Phat sinh", "No cuoi" };
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

                for (int i = 0; i < obj.BaoCaoCongNo.Count(); i++)
                {
                    PdfPCell cell_1 = new PdfPCell(new Phrase(i.ToString()));
                    PdfPCell cell_2 = new PdfPCell(new Phrase(obj.BaoCaoCongNo[i].DaiLy.TenDaiLy.NonUnicode()));
                    PdfPCell cell_3 = new PdfPCell(new Phrase(obj.BaoCaoCongNo[i].NoDau.ToString()));
                    PdfPCell cell_4 = new PdfPCell(new Phrase(obj.BaoCaoCongNo[i].TongPhatSinh.ToString()));
                    PdfPCell cell_5 = new PdfPCell(new Phrase(obj.BaoCaoCongNo[i].NoCuoi.ToString()));

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
                return File(constant, "application/vnd", $"BaoCaoCongNo_{obj.Thang}.pdf");
            }
            return View();
        }
        public IActionResult BaoCaoCongNo()
        {
            return View();
        }
        [HttpPost]
        public IActionResult BaoCaoCongNo(BaoCaoCongNoVM obj)
        {

            BaoCaoCongNoVM baoCaoCongNoVM = new BaoCaoCongNoVM();
            baoCaoCongNoVM.BaoCaoCongNo = new List<BaoCaoCongNo>();

            baoCaoCongNoVM.Thang = obj.Thang;

            //Tạo từng báo cáo công nợ của từng đại lý
            List<DaiLy> daiLys = _db.DaiLys.ToList();
            for (int i = 0; i < daiLys.Count(); i++)
            {
                List<PhieuXuatHang> PhieuXuatHangs = _db.PhieuXuatHangs.Where(c => (c.DaiLyId == daiLys[i].Id && c.NgayLapPhieuXuatHang.Month <= baoCaoCongNoVM.Thang)).ToList();
                List<PhieuThuTien> PhieuThuTiens = _db.PhieuThuTiens.Where(c => (c.IdDaiLy == daiLys[i].Id && c.NgayThuTien.Month <= baoCaoCongNoVM.Thang)).ToList();

                BaoCaoCongNo baoCaoCongNo = new BaoCaoCongNo() { DaiLy = daiLys[i], PhieuXuatHangs = PhieuXuatHangs,PhieuThuTiens=PhieuThuTiens};
                baoCaoCongNoVM.BaoCaoCongNo.Add(baoCaoCongNo);
            }

            //Tính toán nợ đầu, nợ cuối, phát sinh cho từng đại lý
            //Nợ đầu= tổng nhập hàng - tổng tiền thu của các tháng trước đó
            //Phát sinh bằng tổng nhập hàng-tổng tiền thu trong tháng
            //Nợ cuối bằng tổng nhập hàng - tổng tiền thu từ đầu cho tới thời điểm hiện tại
            for (int i = 0; i < baoCaoCongNoVM.BaoCaoCongNo.Count(); i++)
            {
                int NoDau = 0;
                int PhatSinh = 0;
                int NoCuoi = 0;

                foreach (var xuatHang in baoCaoCongNoVM.BaoCaoCongNo[i].PhieuXuatHangs){
                    if (xuatHang.NgayLapPhieuXuatHang.Month < baoCaoCongNoVM.Thang)
                        NoDau += xuatHang.TongTriGia;
                    else
                        PhatSinh += xuatHang.TongTriGia;
                    NoCuoi += xuatHang.TongTriGia;
                }    
                    
                foreach (var thuTien in baoCaoCongNoVM.BaoCaoCongNo[i].PhieuThuTiens){
                    if (thuTien.NgayThuTien.Month < baoCaoCongNoVM.Thang)
                        NoDau -= thuTien.SoTienThu;
                    else
                        PhatSinh -= thuTien.SoTienThu;
                    NoCuoi -= thuTien.SoTienThu;
                }

                baoCaoCongNoVM.BaoCaoCongNo[i].NoDau = NoDau;
                baoCaoCongNoVM.BaoCaoCongNo[i].TongPhatSinh = PhatSinh;
                baoCaoCongNoVM.BaoCaoCongNo[i].NoCuoi = NoCuoi;
            }

            return View(baoCaoCongNoVM);
        }

        //Utility
        public BaoCaoDoanhThuVM GetBaoCaoDoanhThuVM(int thang)
        {
            BaoCaoDoanhThuVM baoCaoDoanhThuVM = new BaoCaoDoanhThuVM();
            baoCaoDoanhThuVM.BaoCaoDoanhThu = new List<BaoCaoDoanhThu>();

            baoCaoDoanhThuVM.Thang = thang;

            List<DaiLy> daiLys = _db.DaiLys.ToList();
            for (int i = 0; i < daiLys.Count(); i++)
            {
                BaoCaoDoanhThu baoCaoDoanhThu = new BaoCaoDoanhThu() { DaiLy = daiLys[i], PhieuXuatHangs = _db.PhieuXuatHangs.Where(c => (c.DaiLyId == daiLys[i].Id) && (c.NgayLapPhieuXuatHang.Month == baoCaoDoanhThuVM.Thang)).ToList() };
                baoCaoDoanhThuVM.BaoCaoDoanhThu.Add(baoCaoDoanhThu);
            }

            //Tính toán tỉ lệ doanh thu
            int TongDoanhThu = 0;

            for (int i = 0; i < baoCaoDoanhThuVM.BaoCaoDoanhThu.Count(); i++)
            {
                for (int j = 0; j < baoCaoDoanhThuVM.BaoCaoDoanhThu[i].PhieuXuatHangs.Count(); j++)
                    //Tính tổng trị giá phiếu xuất hàng của một đại lý
                    baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TongTriGia += baoCaoDoanhThuVM.BaoCaoDoanhThu[i].PhieuXuatHangs[j].TongTriGia;
                //Tính tổng trị giá phiếu xuất hàng của tất cả đại lý
                TongDoanhThu += baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TongTriGia;
            }


            for (int i = 0; i < baoCaoDoanhThuVM.BaoCaoDoanhThu.Count(); i++)
                baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TiLe = ((float)baoCaoDoanhThuVM.BaoCaoDoanhThu[i].TongTriGia / TongDoanhThu) * 100;

            return baoCaoDoanhThuVM;
        }
        public BaoCaoCongNoVM GetBaoCaoCongNoVM(int thang)
        {
            BaoCaoCongNoVM baoCaoCongNoVM = new BaoCaoCongNoVM();
            baoCaoCongNoVM.BaoCaoCongNo = new List<BaoCaoCongNo>();

            baoCaoCongNoVM.Thang = thang;

            //Tạo từng báo cáo công nợ của từng đại lý
            List<DaiLy> daiLys = _db.DaiLys.ToList();
            for (int i = 0; i < daiLys.Count(); i++)
            {
                List<PhieuXuatHang> PhieuXuatHangs = _db.PhieuXuatHangs.Where(c => (c.DaiLyId == daiLys[i].Id && c.NgayLapPhieuXuatHang.Month <= baoCaoCongNoVM.Thang)).ToList();
                List<PhieuThuTien> PhieuThuTiens = _db.PhieuThuTiens.Where(c => (c.IdDaiLy == daiLys[i].Id && c.NgayThuTien.Month <= baoCaoCongNoVM.Thang)).ToList();

                BaoCaoCongNo baoCaoCongNo = new BaoCaoCongNo() { DaiLy = daiLys[i], PhieuXuatHangs = PhieuXuatHangs, PhieuThuTiens = PhieuThuTiens };
                baoCaoCongNoVM.BaoCaoCongNo.Add(baoCaoCongNo);
            }

            //Tính toán nợ đầu, nợ cuối, phát sinh cho từng đại lý
            //Nợ đầu= tổng nhập hàng - tổng tiền thu của các tháng trước đó
            //Phát sinh bằng tổng nhập hàng-tổng tiền thu trong tháng
            //Nợ cuối bằng tổng nhập hàng - tổng tiền thu từ đầu cho tới thời điểm hiện tại
            for (int i = 0; i < baoCaoCongNoVM.BaoCaoCongNo.Count(); i++)
            {
                int NoDau = 0;
                int PhatSinh = 0;
                int NoCuoi = 0;

                foreach (var xuatHang in baoCaoCongNoVM.BaoCaoCongNo[i].PhieuXuatHangs)
                {
                    if (xuatHang.NgayLapPhieuXuatHang.Month < baoCaoCongNoVM.Thang)
                        NoDau += xuatHang.TongTriGia;
                    else
                        PhatSinh += xuatHang.TongTriGia;
                    NoCuoi += xuatHang.TongTriGia;
                }

                foreach (var thuTien in baoCaoCongNoVM.BaoCaoCongNo[i].PhieuThuTiens)
                {
                    if (thuTien.NgayThuTien.Month < baoCaoCongNoVM.Thang)
                        NoDau -= thuTien.SoTienThu;
                    else
                        PhatSinh -= thuTien.SoTienThu;
                    NoCuoi -= thuTien.SoTienThu;
                }

                baoCaoCongNoVM.BaoCaoCongNo[i].NoDau = NoDau;
                baoCaoCongNoVM.BaoCaoCongNo[i].TongPhatSinh = PhatSinh;
                baoCaoCongNoVM.BaoCaoCongNo[i].NoCuoi = NoCuoi;
            }

            return baoCaoCongNoVM;
        }

    }
}
