
giới thiệu vài chức năng cơ bản
	các roles
		Tổng công ty: full chức năng
		Quản lý chi nhánh: check toàn bộ tt
		Quản lý kho: thêm hàng hóa hoặc nhập kho hàng hóa
	đại lý: 
		Tạo
			1: nợ 2tr, 2: nợ 5tr
			SL chi nhánh cùng 1 quận có quy định rõ
		Xem, xuất danh sách pdf
		Tra cứu theo quận, tên
	Xuất hàng
		Xuất hàng	
			Xuất quá số nợ thì ko thành công
			Xuất thành công -> xem chi tiết xuất pdf -> nợ đại lý được tăng lên
	thu tiền
		thu nhiều hơn số nợ thì thất bại
		thu vừa đủ thì thành công-> nợ giảm, xuất pdf phiếu thu
	Báo cáo
		Doanh thu: chọn tháng 2 và xuất pdf
		Công nợ: chọn tháng 2, nợ đầu: đầu tháng, nợ cuối: hiện tại hoặc kết tháng.
	Hàng hóa
		Chỉnh sửa: chỉnh sl thôi chứ chỉnh giá là bị bug
		Thêm mặt hàng
	Thêm tài khoản
		Cái này nói sơ sơ

=================run this for init data=========================
INSERT INTO QuyDinhs(MaNhanDien,NoiDung,GiaTri,Changeable) VALUES
('SL_DL_TDQ',N'Số lượng đại lý tối đa trong một quận:',1,1),
('SL_MH',N'Số lượng mặt hàng',5,1),
('SL_DVT',N'Số lượng đơn vị tính',3,1),
('TN_DL1',N'Số tiền nợ đại lý 1',2000000,1),
('TN_DL2',N'Số tiền nợ đại lý 2',5000000,1)

INSERT INTO HangHoas(TenHang,DonViTinh,SoLuongTrongKho,DonGia) VALUES
(N'Bitis Hunter Street Retro Collection',N'Đôi', 3000,830000),
(N'BitiS Hunter X Cut Out',N'Đôi',5000,1010000),
(N'EMBROIDERED UNI NOWSG TEE',N'Cái',4000,365000),
(N'DAILY BACKPACK - LEATHER',N'Cái',7000,580000),
(N'OG LOGO SOCKS',N'Cặp',4000,95000)
