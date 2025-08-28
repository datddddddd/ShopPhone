-- =============================================
-- Script tạo dữ liệu demo cho Shop Phone
-- Bao gồm: Tài khoản demo, Thẻ tín dụng mẫu, Ví điện tử mẫu
-- =============================================

-- USE [ShopPhone] -- Bỏ comment và thay đổi tên database nếu cần
-- Hoặc chọn database trong SSMS trước khi chạy script
-- GO

-- =============================================
-- 1. TẠO TÀI KHOẢN DEMO
-- =============================================

-- Xóa dữ liệu cũ nếu có (tùy chọn)
-- DELETE FROM ViDienTu WHERE TaiKhoanId IN (SELECT Id FROM TaiKhoan WHERE Email LIKE '%demo%')
-- DELETE FROM TaiKhoan WHERE Email LIKE '%demo%'

-- Tạo tài khoản demo
INSERT INTO TaiKhoan (MatKhau, VaiTro, Email, NgayTao, HoTen, AnhDaiDien)
VALUES 
    -- Mật khẩu: demo123 (nên hash trong thực tế)
    ('demo123', 'User', 'demo1@shopphone.com', GETDATE(), N'Nguyễn Văn Demo', NULL),
    ('demo123', 'User', 'demo2@shopphone.com', GETDATE(), N'Trần Thị Demo', NULL),
    ('demo123', 'User', 'demo3@shopphone.com', GETDATE(), N'Lê Văn Test', NULL),
    ('demo123', 'User', 'demo4@shopphone.com', GETDATE(), N'Phạm Thị Test', NULL),
    ('demo123', 'User', 'demo5@shopphone.com', GETDATE(), N'Hoàng Văn Sample', NULL),
    -- Tài khoản admin demo
    ('admin123', 'Admin', 'admin@shopphone.com', GETDATE(), N'Admin Demo', NULL);

-- =============================================
-- 2. TẠO THẺ TÍN DỤNG MẪU THÊM
-- =============================================

-- Thêm các thẻ tín dụng test mới (ngoài những thẻ đã có sẵn)
INSERT INTO TheTinDung (SoThe, ChuThe, NgayHetHan, CVV, LoaiThe, HanMuc, HoatDong, NgayTao)
VALUES 
    -- Thẻ Visa
    ('4000000000000077', N'DEMO USER 1', '03/26', '111', 'Visa', 9999999.99, 1, '2024-01-01'),
    ('4000000000000085', N'DEMO USER 2', '04/26', '222', 'Visa', 9999999.99, 1, '2024-01-01'),
    ('4000000000000093', N'DEMO USER 3', '05/26', '333', 'Visa', 9999999.99, 1, '2024-01-01'),
    
    -- Thẻ MasterCard
    ('5200000000000007', N'DEMO USER 4', '06/26', '444', 'MasterCard', 9999999.99, 1, '2024-01-01'),
    ('5200000000000015', N'DEMO USER 5', '07/26', '555', 'MasterCard', 9999999.99, 1, '2024-01-01'),
    ('5200000000000023', N'DEMO USER 6', '08/26', '666', 'MasterCard', 9999999.99, 1, '2024-01-01'),
    
    -- Thẻ American Express
    ('340000000000009', N'DEMO USER 7', '09/26', '777', 'American Express', 9999999.99, 1, '2024-01-01'),
    ('340000000000017', N'DEMO USER 8', '10/26', '888', 'American Express', 9999999.99, 1, '2024-01-01'),
    
    -- Thẻ JCB
    ('3530111333300001', N'DEMO USER 9', '11/26', '999', 'JCB', 9999999.99, 1, '2024-01-01'),
    ('3530111333300019', N'DEMO USER 10', '12/26', '000', 'JCB', 9999999.99, 1, '2024-01-01'),
    
    -- Thẻ Discover
    ('6011000990139432', N'DEMO USER 11', '01/27', '123', 'Discover', 9999999.99, 1, '2024-01-01'),
    ('6011000990139440', N'DEMO USER 12', '02/27', '456', 'Discover', 9999999.99, 1, '2024-01-01');

-- =============================================
-- 3. TẠO VÍ ĐIỆN TỬ MẪU
-- =============================================

-- Lấy ID của các tài khoản demo vừa tạo
DECLARE @DemoUser1 INT = (SELECT Id FROM TaiKhoan WHERE Email = 'demo1@shopphone.com')
DECLARE @DemoUser2 INT = (SELECT Id FROM TaiKhoan WHERE Email = 'demo2@shopphone.com')
DECLARE @DemoUser3 INT = (SELECT Id FROM TaiKhoan WHERE Email = 'demo3@shopphone.com')
DECLARE @DemoUser4 INT = (SELECT Id FROM TaiKhoan WHERE Email = 'demo4@shopphone.com')
DECLARE @DemoUser5 INT = (SELECT Id FROM TaiKhoan WHERE Email = 'demo5@shopphone.com')

-- Tạo ví MoMo demo
INSERT INTO ViDienTu (TenChuVi, SoDienThoai, LoaiVi, TaiKhoanId)
VALUES 
    (N'Nguyễn Văn Demo', '0901234567', 'Momo', @DemoUser1),
    (N'Trần Thị Demo', '0902345678', 'Momo', @DemoUser2),
    (N'Lê Văn Test', '0903456789', 'Momo', @DemoUser3),
    (N'Phạm Thị Test', '0904567890', 'Momo', @DemoUser4),
    (N'Hoàng Văn Sample', '0905678901', 'Momo', @DemoUser5),
    
    -- Thêm ví ZaloPay demo
    (N'Nguyễn Văn Demo', '0901234567', 'ZaloPay', @DemoUser1),
    (N'Trần Thị Demo', '0902345678', 'ZaloPay', @DemoUser2),
    
    -- Thêm ví ViettelPay demo
    (N'Lê Văn Test', '0903456789', 'ViettelPay', @DemoUser3),
    (N'Phạm Thị Test', '0904567890', 'ViettelPay', @DemoUser4),
    
    -- Thêm ví ShopeePay demo
    (N'Hoàng Văn Sample', '0905678901', 'ShopeePay', @DemoUser5);

-- =============================================
-- 4. THÊM SẢN PHẨM DEMO (TÙY CHỌN)
-- =============================================

-- Thêm một số sản phẩm demo nếu chưa có
IF NOT EXISTS (SELECT 1 FROM HangHoa WHERE TenHH LIKE '%Demo%')
BEGIN
    INSERT INTO HangHoa (TenHH, TenAlias, MaLoai, MoTaDonVi, DonGia, GiamGia, Hinh, NgaySX, SoLanXem, MoTa, MaNCC, DanhGia, HinhMoHop, HinhThucTe, VideoId)
    VALUES
        (N'iPhone 15 Demo', N'iphone-15-demo', 1, N'Chiếc', 25000000, 10, 'iphone15.jpg', '2024-01-01', 0, N'iPhone 15 phiên bản demo cho test', 'APPLE', 4.5, NULL, NULL, NULL),
        (N'Samsung Galaxy S24 Demo', N'samsung-galaxy-s24-demo', 1, N'Chiếc', 22000000, 15, 'galaxys24.jpg', '2024-01-01', 0, N'Samsung Galaxy S24 phiên bản demo', 'SAMSUNG', 4.3, NULL, NULL, NULL),
        (N'Xiaomi 14 Demo', N'xiaomi-14-demo', 1, N'Chiếc', 15000000, 20, 'xiaomi14.jpg', '2024-01-01', 0, N'Xiaomi 14 phiên bản demo', 'XIAOMI', 4.2, NULL, NULL, NULL),
        (N'OnePlus 12 Demo', N'oneplus-12-demo', 1, N'Chiếc', 18000000, 12, 'oneplus12.jpg', '2024-01-01', 0, N'OnePlus 12 phiên bản demo', 'ONEPLUS', 4.4, NULL, NULL, NULL);
END

-- =============================================
-- 5. HIỂN THỊ THÔNG TIN TÀI KHOẢN DEMO
-- =============================================

PRINT '============================================='
PRINT 'THÔNG TIN TÀI KHOẢN DEMO ĐÃ TẠO'
PRINT '============================================='

-- Hiển thị tài khoản demo
SELECT
    Id,
    Email,
    HoTen,
    VaiTro,
    NgayTao
FROM TaiKhoan
WHERE Email LIKE '%demo%' OR Email LIKE '%admin@shopphone.com%'
ORDER BY Id

PRINT ''
PRINT '============================================='
PRINT 'THÔNG TIN THẺ TÍN DỤNG TEST'
PRINT '============================================='

-- Hiển thị thẻ tín dụng test
SELECT
    SoThe,
    ChuThe,
    NgayHetHan,
    CVV,
    LoaiThe
FROM TheTinDung
WHERE HoatDong = 1
ORDER BY LoaiThe, Id

PRINT ''
PRINT '============================================='
PRINT 'THÔNG TIN VÍ ĐIỆN TỬ TEST'
PRINT '============================================='

-- Hiển thị ví điện tử test
SELECT
    vd.TenChuVi,
    vd.SoDienThoai,
    vd.LoaiVi,
    tk.Email as EmailTaiKhoan
FROM ViDienTu vd
INNER JOIN TaiKhoan tk ON vd.TaiKhoanId = tk.Id
ORDER BY vd.LoaiVi, vd.Id

PRINT ''
PRINT '============================================='
PRINT 'HƯỚNG DẪN SỬ DỤNG'
PRINT '============================================='
PRINT 'Tài khoản đăng nhập:'
PRINT '- Email: demo1@shopphone.com | Mật khẩu: demo123'
PRINT '- Email: demo2@shopphone.com | Mật khẩu: demo123'
PRINT '- Email: admin@shopphone.com | Mật khẩu: admin123 (Admin)'
PRINT ''
PRINT 'Thẻ tín dụng test phổ biến:'
PRINT '- Visa: 4111111111111111 | Chủ thẻ: TEST USER | MM/YY: 12/25 | CVV: 123'
PRINT '- MasterCard: 5555555555554444 | Chủ thẻ: TEST USER | MM/YY: 12/25 | CVV: 123'
PRINT ''
PRINT 'Ví MoMo test:'
PRINT '- SĐT: 0909123456 | Tên: Test Momo'
PRINT '- SĐT: 0901234567 | Tên: Nguyễn Văn Demo'
PRINT '============================================='

GO
