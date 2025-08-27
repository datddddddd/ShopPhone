-- =============================================
-- Script tối thiểu tạo tài khoản demo
-- Chỉ tạo tài khoản và ví MoMo cơ bản
-- =============================================

-- Tạo tài khoản demo với mật khẩu plain text (sẽ được xử lý trong code)
INSERT INTO TaiKhoan (MatKhau, VaiTro, Email, NgayTao, HoTen)
VALUES
    ('demo123', 'User', 'demo@shopphone.com', GETDATE(), N'Demo User');

-- Lấy ID tài khoản vừa tạo
DECLARE @UserId INT = SCOPE_IDENTITY();

-- Tạo ví MoMo cho tài khoản demo
INSERT INTO ViDienTu (TenChuVi, SoDienThoai, LoaiVi, TaiKhoanId)
VALUES 
    (N'Demo User', '0912345678', 'Momo', @UserId);

-- Hiển thị kết quả
SELECT 'Tạo tài khoản demo thành công!' as Message;
SELECT Email, HoTen, VaiTro FROM TaiKhoan WHERE Id = @UserId;
SELECT TenChuVi, SoDienThoai, LoaiVi FROM ViDienTu WHERE TaiKhoanId = @UserId;

PRINT 'Đăng nhập với: demo@shopphone.com / demo123'
PRINT 'Ví MoMo test: 0912345678 / Demo User'
