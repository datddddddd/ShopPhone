-- =============================================
-- Script kiểm tra cấu trúc database Shop Phone
-- =============================================

-- Kiểm tra các bảng có tồn tại không
PRINT '============================================='
PRINT 'KIỂM TRA CÁC BẢNG TRONG DATABASE'
PRINT '============================================='

SELECT 
    TABLE_NAME as 'Tên Bảng',
    TABLE_TYPE as 'Loại'
FROM INFORMATION_SCHEMA.TABLES 
WHERE TABLE_TYPE = 'BASE TABLE'
ORDER BY TABLE_NAME

PRINT ''
PRINT '============================================='
PRINT 'CẤU TRÚC BẢNG HANGHOA'
PRINT '============================================='

SELECT 
    COLUMN_NAME as 'Tên Cột',
    DATA_TYPE as 'Kiểu Dữ Liệu',
    IS_NULLABLE as 'Cho Phép NULL',
    COLUMN_DEFAULT as 'Giá Trị Mặc Định',
    CHARACTER_MAXIMUM_LENGTH as 'Độ Dài Tối Đa'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'HangHoa'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '============================================='
PRINT 'CẤU TRÚC BẢNG TAIKHOAN'
PRINT '============================================='

SELECT 
    COLUMN_NAME as 'Tên Cột',
    DATA_TYPE as 'Kiểu Dữ Liệu',
    IS_NULLABLE as 'Cho Phép NULL',
    COLUMN_DEFAULT as 'Giá Trị Mặc Định'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'TaiKhoan'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '============================================='
PRINT 'CẤU TRÚC BẢNG THETINDUNG'
PRINT '============================================='

SELECT 
    COLUMN_NAME as 'Tên Cột',
    DATA_TYPE as 'Kiểu Dữ Liệu',
    IS_NULLABLE as 'Cho Phép NULL'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'TheTinDung'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '============================================='
PRINT 'CẤU TRÚC BẢNG VIDIENTU'
PRINT '============================================='

SELECT 
    COLUMN_NAME as 'Tên Cột',
    DATA_TYPE as 'Kiểu Dữ Liệu',
    IS_NULLABLE as 'Cho Phép NULL'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'ViDienTu'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '============================================='
PRINT 'CẤU TRÚC BẢNG PHUONGTHUCTHANHTOAN'
PRINT '============================================='

SELECT 
    COLUMN_NAME as 'Tên Cột',
    DATA_TYPE as 'Kiểu Dữ Liệu',
    IS_NULLABLE as 'Cho Phép NULL'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'PhuongThucThanhToan'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '============================================='
PRINT 'CẤU TRÚC BẢNG PHUONGTHUCGIAOHANG'
PRINT '============================================='

SELECT 
    COLUMN_NAME as 'Tên Cột',
    DATA_TYPE as 'Kiểu Dữ Liệu',
    IS_NULLABLE as 'Cho Phép NULL'
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'PhuongThucGiaoHang'
ORDER BY ORDINAL_POSITION

PRINT ''
PRINT '============================================='
PRINT 'DỮ LIỆU CÓ SẴN TRONG CÁC BẢNG'
PRINT '============================================='

SELECT 'TaiKhoan' as BangTen, COUNT(*) as SoLuong FROM TaiKhoan
UNION ALL
SELECT 'HangHoa', COUNT(*) FROM HangHoa
UNION ALL
SELECT 'TheTinDung', COUNT(*) FROM TheTinDung
UNION ALL
SELECT 'ViDienTu', COUNT(*) FROM ViDienTu
UNION ALL
SELECT 'PhuongThucThanhToan', COUNT(*) FROM PhuongThucThanhToan
UNION ALL
SELECT 'PhuongThucGiaoHang', COUNT(*) FROM PhuongThucGiaoHang

PRINT ''
PRINT '============================================='
PRINT 'PHƯƠNG THỨC THANH TOÁN CÓ SẴN'
PRINT '============================================='

SELECT Id, Ten, MoTa, Icon, HoatDong, YeuCauTheTinDung 
FROM PhuongThucThanhToan 
ORDER BY ThuTu

PRINT ''
PRINT '============================================='
PRINT 'PHƯƠNG THỨC GIAO HÀNG CÓ SẴN'
PRINT '============================================='

SELECT Id, Ten, MoTa, PhiGiaoHang, HoatDong 
FROM PhuongThucGiaoHang 
ORDER BY ThuTu
