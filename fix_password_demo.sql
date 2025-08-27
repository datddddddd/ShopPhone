-- =============================================
-- Script sửa lỗi mật khẩu cho tài khoản demo
-- Cập nhật mật khẩu thành plain text để tránh lỗi Base64
-- =============================================

-- Cập nhật mật khẩu cho tài khoản demo thành plain text
UPDATE TaiKhoan 
SET MatKhau = 'demo123' 
WHERE Email = 'demo@shopphone.com';

UPDATE TaiKhoan 
SET MatKhau = 'admin123' 
WHERE Email = 'admin@shopphone.com';

-- Cập nhật các tài khoản demo khác nếu có
UPDATE TaiKhoan 
SET MatKhau = 'demo123' 
WHERE Email LIKE '%demo%@shopphone.com';

-- Hiển thị kết quả
SELECT 
    Email,
    HoTen,
    VaiTro,
    CASE 
        WHEN LEN(MatKhau) < 20 THEN 'Plain Text Password'
        ELSE 'Hashed Password'
    END as PasswordType
FROM TaiKhoan 
WHERE Email LIKE '%demo%' OR Email LIKE '%admin@shopphone.com%'
ORDER BY Id;

PRINT 'Đã cập nhật mật khẩu cho tài khoản demo thành plain text'
PRINT 'Bây giờ có thể đăng nhập với:'
PRINT 'Email: demo@shopphone.com | Mật khẩu: demo123'
PRINT 'Email: admin@shopphone.com | Mật khẩu: admin123'
