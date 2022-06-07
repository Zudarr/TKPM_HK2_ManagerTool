using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TKPM.Data;
using TKPM.Models;
using TKPM.Models.ViewModels;

namespace TKPM.Controllers
{
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
    }
}
