-- =============================================
-- Script nhanh tạo tài khoản demo cho Shop Phone
-- =============================================

-- Tạo tài khoản demo
INSERT INTO TaiKhoan (MatKhau, VaiTro, Email, NgayTao, HoTen, AnhDaiDien)
VALUES
    ('demo123', 'User', 'demo@shopphone.com', GETDATE(), N'Tài Khoản Demo', NULL),
    ('admin123', 'Admin', 'admin@shopphone.com', GETDATE(), N'Admin Demo', NULL);

-- Lấy ID tài khoản demo
DECLARE @DemoUserId INT = (SELECT Id FROM TaiKhoan WHERE Email = 'demo@shopphone.com')

-- Tạo ví MoMo cho tài khoản demo
INSERT INTO ViDienTu (TenChuVi, SoDienThoai, LoaiVi, TaiKhoanId)
VALUES
    (N'Tài Khoản Demo', '0912345678', 'Momo', @DemoUserId);

-- Thêm sản phẩm demo đơn giản
IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE TenHH = N'iPhone 15 Demo')
BEGIN
    INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, GiamGia, Hinh, NgaySX, SoLanXem, MoTa, MaNCC, DanhGia)
    VALUES
        (N'iPhone 15 Demo', N'iphone-15-demo', 1, N'Chiếc', 25000000, 10, 'iphone15.jpg', '2024-01-01', 0, N'iPhone 15 demo', 'APPLE', 4.5);
END

-- Hiển thị thông tin
SELECT 'Tài khoản demo đã được tạo!' as Message
SELECT Email, HoTen, VaiTro FROM TaiKhoan WHERE Email IN ('demo@shopphone.com', 'admin@shopphone.com')
SELECT TenChuVi, SoDienThoai, LoaiVi FROM ViDienTu WHERE TaiKhoanId = @DemoUserId

PRINT '============================================='
PRINT 'THÔNG TIN ĐĂNG NHẬP:'
PRINT 'Email: demo@shopphone.com'
PRINT 'Mật khẩu: demo123'
PRINT ''
PRINT 'Admin:'
PRINT 'Email: admin@shopphone.com' 
PRINT 'Mật khẩu: admin123'
PRINT ''
PRINT 'Ví MoMo test:'
PRINT 'SĐT: 0912345678'
PRINT 'Tên: Tài Khoản Demo'
PRINT ''
PRINT 'Thẻ test có sẵn:'
PRINT 'Visa: 4111111111111111 | TEST USER | 12/25 | 123'
PRINT 'MasterCard: 5555555555554444 | TEST USER | 12/25 | 123'
PRINT '============================================='
