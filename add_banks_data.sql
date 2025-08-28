-- Script thêm dữ liệu ngân hàng cho thanh toán thẻ tín dụng

-- Tạo bảng NganHang nếu chưa có
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='NganHang' AND xtype='U')
BEGIN
    CREATE TABLE [dbo].[NganHang] (
        [Id] int IDENTITY(1,1) NOT NULL,
        [TenNganHang] nvarchar(100) NOT NULL,
        [MaNganHang] nvarchar(10) NULL,
        [Logo] nvarchar(200) NULL,
        [LoaiThe] nvarchar(50) NULL,
        [MauThe] nvarchar(20) NULL,
        [MoTa] nvarchar(500) NULL,
        [HoatDong] bit NOT NULL DEFAULT 1,
        [ThuTu] int NOT NULL DEFAULT 0,
        [NgayTao] datetime2(7) NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_NganHang] PRIMARY KEY CLUSTERED ([Id] ASC)
    )
END

-- Xóa dữ liệu cũ nếu có
DELETE FROM [dbo].[NganHang]

-- Thêm dữ liệu ngân hàng
INSERT INTO [dbo].[NganHang] ([TenNganHang], [MaNganHang], [Logo], [LoaiThe], [MauThe], [MoTa], [HoatDong], [ThuTu])
VALUES
-- Ngân hàng Việt Nam
('Vietcombank', 'VCB', '/Banks/vietcombank.jpg', 'Visa', '#007bff', 'Ngân hàng Ngoại thương Việt Nam', 1, 1),
('Techcombank', 'TCB', '/Banks/Techcombank.png', 'MasterCard', '#eb001b', 'Ngân hàng Kỹ thương Việt Nam', 1, 2),
('BIDV', 'BIDV', '/Banks/Bidv.jpg', 'JCB', '#0066cc', 'Ngân hàng Đầu tư và Phát triển Việt Nam', 1, 3),
('VietinBank', 'VTB', '/Banks/VietinBank.png', 'Visa', '#1e88e5', 'Ngân hàng Công thương Việt Nam', 1, 4),
('Agribank', 'AGB', '/Banks/Agribank.png', 'MasterCard', '#4caf50', 'Ngân hàng Nông nghiệp và Phát triển Nông thôn', 1, 5),
('ACB', 'ACB', '/Banks/Acb.png', 'Visa', '#ff5722', 'Ngân hàng Á Châu', 1, 6),
('Sacombank', 'STB', '/Banks/sacombank.png', 'MasterCard', '#9c27b0', 'Ngân hàng TMCP Sài Gòn Thương Tín', 1, 7),
('VPBank', 'VPB', '/Banks/VPBank.png', 'JCB', '#ff9800', 'Ngân hàng Việt Nam Thịnh Vượng', 1, 8),
('MBBank', 'MBB', '/Banks/MBBank.jpg', 'Visa', '#795548', 'Ngân hàng Quân đội', 1, 9),
('TPBank', 'TPB', '/Banks/TPBank.webp', 'MasterCard', '#e91e63', 'Ngân hàng Tiên Phong', 1, 10),

-- Ngân hàng quốc tế
('HSBC', 'HSBC', '/Banks/HSBC.png', 'Visa', '#dc143c', 'Hongkong and Shanghai Banking Corporation', 1, 11),
('Standard Chartered', 'SCB', '/Banks/standardchartered.jpg', 'MasterCard', '#0f4c81', 'Standard Chartered Bank', 1, 12),
('Citibank', 'CITI', '/Banks/citibank.png', 'JCB', '#004b87', 'Citibank Vietnam', 1, 13),
('ANZ', 'ANZ', '/Banks/ANZBank.png', 'Visa', '#005aa3', 'Australia and New Zealand Banking Group', 1, 14),
('Shinhan Bank', 'SHB', '/Banks/shinhanbank.png', 'MasterCard', '#0066cc', 'Shinhan Bank Vietnam', 1, 15);

-- Thêm dữ liệu thẻ tín dụng test cho các ngân hàng
INSERT INTO [dbo].[TheTinDung] ([SoThe], [ChuThe], [NgayHetHan], [CVV], [HoatDong])
VALUES 
-- Visa cards
('4111111111111111', 'TEST USER', '12/25', '123', 1),
('4000000000000002', 'DEMO VISA', '12/26', '456', 1),
('4242424242424242', 'VCB VISA', '12/27', '789', 1),

-- MasterCard cards  
('5555555555554444', 'TEST USER', '12/25', '123', 1),
('5200000000000007', 'DEMO MASTER', '12/26', '456', 1),
('5105105105105100', 'TCB MASTER', '12/27', '789', 1),

-- JCB cards
('3530111333300000', 'TEST USER', '12/25', '123', 1),
('3566002020360505', 'DEMO JCB', '12/26', '456', 1),
('3528000700000000', 'BIDV JCB', '12/27', '789', 1);

-- Hiển thị kết quả
SELECT 'Đã thêm ' + CAST(COUNT(*) AS VARCHAR) + ' ngân hàng' AS KetQua
FROM [dbo].[NganHang]

SELECT 'Đã có ' + CAST(COUNT(*) AS VARCHAR) + ' thẻ test' AS KetQua  
FROM [dbo].[TheTinDung]

-- Hiển thị danh sách ngân hàng
SELECT 
    Id,
    TenNganHang,
    MaNganHang,
    LoaiThe,
    MauThe,
    HoatDong,
    ThuTu
FROM [dbo].[NganHang]
ORDER BY ThuTu

PRINT 'Hoàn thành thêm dữ liệu ngân hàng!'
PRINT 'Bạn có thể chạy migration để tạo bảng NganHang trong Entity Framework:'
PRINT 'Add-Migration AddNganHangTable'
PRINT 'Update-Database'
