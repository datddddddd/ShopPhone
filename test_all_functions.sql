-- =============================================
-- Script test toàn diện các chức năng Shop Phone
-- =============================================

-- 1. Kiểm tra cấu trúc database
PRINT '============================================='
PRINT '1. KIỂM TRA CẤU TRÚC DATABASE'
PRINT '============================================='

SELECT 'Bảng TaiKhoan' as BangTen, COUNT(*) as SoLuong FROM TaiKhoan
UNION ALL
SELECT 'Bảng HangHoa', COUNT(*) FROM HangHoa
UNION ALL
SELECT 'Bảng TheTinDung', COUNT(*) FROM TheTinDung
UNION ALL
SELECT 'Bảng ViDienTu', COUNT(*) FROM ViDienTu
UNION ALL
SELECT 'Bảng PhuongThucThanhToan', COUNT(*) FROM PhuongThucThanhToan
UNION ALL
SELECT 'Bảng PhuongThucGiaoHang', COUNT(*) FROM PhuongThucGiaoHang

-- 2. Tạo tài khoản test nếu chưa có
PRINT ''
PRINT '============================================='
PRINT '2. TẠO TÀI KHOẢN TEST'
PRINT '============================================='

IF NOT EXISTS (SELECT 1 FROM TaiKhoan WHERE Email = 'test@shopphone.com')
BEGIN
    INSERT INTO TaiKhoan (MatKhau, VaiTro, Email, NgayTao, HoTen)
    VALUES ('test123', 'User', 'test@shopphone.com', GETDATE(), N'Test User');
    
    DECLARE @TestUserId INT = SCOPE_IDENTITY();
    
    -- Tạo ví MoMo cho tài khoản test
    INSERT INTO ViDienTu (TenChuVi, SoDienThoai, LoaiVi, TaiKhoanId)
    VALUES (N'Test User', '0987654321', 'Momo', @TestUserId);
    
    PRINT 'Đã tạo tài khoản test: test@shopphone.com / test123'
END
ELSE
BEGIN
    PRINT 'Tài khoản test đã tồn tại'
END

-- 3. Kiểm tra phương thức thanh toán
PRINT ''
PRINT '============================================='
PRINT '3. PHƯƠNG THỨC THANH TOÁN'
PRINT '============================================='

SELECT 
    Id,
    Ten,
    MoTa,
    CASE WHEN HoatDong = 1 THEN 'Hoạt động' ELSE 'Không hoạt động' END as TrangThai,
    CASE WHEN YeuCauTheTinDung = 1 THEN 'Có' ELSE 'Không' END as YeuCauThe
FROM PhuongThucThanhToan
ORDER BY ThuTu

-- 4. Kiểm tra phương thức giao hàng
PRINT ''
PRINT '============================================='
PRINT '4. PHƯƠNG THỨC GIAO HÀNG'
PRINT '============================================='

SELECT 
    Id,
    Ten,
    MoTa,
    PhiGiaoHang,
    CASE WHEN HoatDong = 1 THEN 'Hoạt động' ELSE 'Không hoạt động' END as TrangThai
FROM PhuongThucGiaoHang
ORDER BY ThuTu

-- 5. Kiểm tra thẻ tín dụng test
PRINT ''
PRINT '============================================='
PRINT '5. THẺ TÍN DỤNG TEST'
PRINT '============================================='

SELECT 
    SoThe,
    ChuThe,
    NgayHetHan,
    CVV,
    LoaiThe,
    CASE WHEN HoatDong = 1 THEN 'Hoạt động' ELSE 'Không hoạt động' END as TrangThai
FROM TheTinDung
WHERE HoatDong = 1
ORDER BY LoaiThe

-- 6. Kiểm tra ví điện tử test
PRINT ''
PRINT '============================================='
PRINT '6. VÍ ĐIỆN TỬ TEST'
PRINT '============================================='

SELECT 
    vd.TenChuVi,
    vd.SoDienThoai,
    vd.LoaiVi,
    tk.Email as EmailTaiKhoan
FROM ViDienTu vd
INNER JOIN TaiKhoan tk ON vd.TaiKhoanId = tk.Id
ORDER BY vd.LoaiVi

-- 7. Thêm sản phẩm test nếu chưa có
PRINT ''
PRINT '============================================='
PRINT '7. SẢN PHẨM TEST'
PRINT '============================================='

IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE TenHH = N'iPhone Test')
BEGIN
    INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, GiamGia, Hinh, NgaySX, SoLanXem, MoTa, MaNCC, DanhGia)
    VALUES 
        (N'iPhone Test', N'iphone-test', 1, N'Chiếc', 20000000, 5, 'iphone-test.jpg', '2024-01-01', 0, N'iPhone test cho demo', 'APPLE', 4.5);
    PRINT 'Đã thêm sản phẩm test'
END
ELSE
BEGIN
    PRINT 'Sản phẩm test đã tồn tại'
END

-- Hiển thị một số sản phẩm
SELECT TOP 5
    MaHH,
    TenHH,
    DonGia,
    GiamGia,
    MoTa
FROM HangHoa
ORDER BY MaHH DESC

-- 8. Tóm tắt kết quả
PRINT ''
PRINT '============================================='
PRINT '8. TÓM TẮT KIỂM TRA'
PRINT '============================================='

DECLARE @TaiKhoanCount INT = (SELECT COUNT(*) FROM TaiKhoan)
DECLARE @HangHoaCount INT = (SELECT COUNT(*) FROM HangHoa)
DECLARE @TheTinDungCount INT = (SELECT COUNT(*) FROM TheTinDung WHERE HoatDong = 1)
DECLARE @ViDienTuCount INT = (SELECT COUNT(*) FROM ViDienTu)

PRINT 'Số tài khoản: ' + CAST(@TaiKhoanCount AS VARCHAR(10))
PRINT 'Số sản phẩm: ' + CAST(@HangHoaCount AS VARCHAR(10))
PRINT 'Số thẻ tín dụng test: ' + CAST(@TheTinDungCount AS VARCHAR(10))
PRINT 'Số ví điện tử test: ' + CAST(@ViDienTuCount AS VARCHAR(10))

PRINT ''
PRINT '============================================='
PRINT 'HƯỚNG DẪN TEST ỨNG DỤNG'
PRINT '============================================='
PRINT '1. Đăng nhập: test@shopphone.com / test123'
PRINT '2. Thêm sản phẩm vào giỏ hàng'
PRINT '3. Vào trang thanh toán'
PRINT '4. Test thanh toán với thẻ: 4111111111111111 | TEST USER | 12/25 | 123'
PRINT '5. Test thanh toán với MoMo: 0987654321 | Test User'
PRINT '6. Test giao hàng tại nhà (30,000 VNĐ) và tại shop (0 VNĐ)'
PRINT '============================================='
