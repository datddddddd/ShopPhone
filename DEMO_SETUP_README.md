# Hướng dẫn thiết lập dữ liệu demo cho Shop Phone

## 📋 Tổng quan

Dự án Shop Phone đã có đầy đủ các chức năng thanh toán và giao hàng. Các file SQL này giúp tạo dữ liệu demo để test các tính năng.

## 🚀 Cách sử dụng

### Option 1: Setup nhanh (Khuyến nghị)

```sql
-- Chạy file quick_demo_setup.sql
-- Tạo 2 tài khoản demo cơ bản với ví MoMo
```

### Option 2: Setup đầy đủ

```sql
-- Chạy file demo_data.sql
-- Tạo nhiều tài khoản demo với đầy đủ thẻ tín dụng và ví điện tử
```

## 🔑 Thông tin đăng nhập demo

### Tài khoản User

- **Email:** `demo@shopphone.com`
- **Mật khẩu:** `demo123`

### Tài khoản Admin

- **Email:** `admin@shopphone.com`
- **Mật khẩu:** `admin123`

## 💳 Thông tin thanh toán test

### Thẻ tín dụng test (có sẵn trong hệ thống)

| Loại thẻ   | Số thẻ           | Chủ thẻ     | MM/YY | CVV |
| ---------- | ---------------- | ----------- | ----- | --- |
| Visa       | 4111111111111111 | TEST USER   | 12/25 | 123 |
| MasterCard | 5555555555554444 | TEST USER   | 12/25 | 123 |
| Visa       | 4000000000000002 | TEST USER 2 | 11/26 | 456 |
| MasterCard | 5105105105105100 | TEST USER 3 | 10/27 | 789 |

### Ví điện tử test

| Loại ví | Số điện thoại | Tên chủ ví     |
| ------- | ------------- | -------------- |
| MoMo    | 0909123456    | Test Momo      |
| MoMo    | 0912345678    | Tài Khoản Demo |

## 🚚 Phương thức giao hàng có sẵn

1. **Giao hàng tại nhà**

   - Phí: 30,000 VNĐ
   - Thời gian: 2-3 ngày

2. **Nhận tại shop**
   - Phí: 0 VNĐ (miễn phí)
   - Thời gian: Ngay khi có hàng

## 💰 Phương thức thanh toán có sẵn

1. **Tiền mặt**
   - Thanh toán khi nhận hàng (COD)
2. **Thẻ tín dụng/ghi nợ**
   - Hỗ trợ: Visa, MasterCard, JCB, Discover
   - Có validation thông tin thẻ
3. **Ví điện tử MoMo**
   - Nhập số điện thoại và tên chủ ví
   - Có validation thông tin ví

## 🔧 Cách chạy script

### Bước 1: Kiểm tra cấu trúc database (Khuyến nghị)

```sql
-- Chạy file check_database_structure.sql trước để kiểm tra cấu trúc
-- Đảm bảo các bảng đã được tạo đúng
```

### Bước 2: Chạy script demo

#### Sử dụng SQL Server Management Studio (SSMS)

1. Mở SSMS và kết nối đến SQL Server
2. Chọn database `ShopPhone` (hoặc tên database của bạn)
3. Mở file `quick_demo_setup.sql` hoặc `demo_data.sql`
4. **QUAN TRỌNG:** Không cần thay đổi `USE [ShopPhoneDB]` - chỉ cần chọn đúng database
5. Chạy script (F5)

#### Sử dụng Entity Framework

```bash
# Đảm bảo database đã được tạo
dotnet ef database update --project ShopPhone
# Sau đó chạy script SQL để thêm dữ liệu demo
```

### ⚠️ Lưu ý quan trọng về lỗi

- **Lỗi "Invalid column name"**: Đảm bảo đã chạy migration đầy đủ
- **Lỗi bảng không tồn tại**: Chạy `dotnet ef database update` trước
- **Lỗi duplicate key**: Xóa dữ liệu cũ trước khi chạy script mới

## 📱 Test flow hoàn chỉnh

1. **Đăng nhập:** Sử dụng tài khoản demo
2. **Thêm sản phẩm vào giỏ hàng**
3. **Vào trang thanh toán:** `/ThanhToan`
4. **Chọn phương thức giao hàng:** Tại nhà hoặc tại shop
5. **Chọn phương thức thanh toán:** Tiền mặt, thẻ, hoặc MoMo
6. **Điền thông tin:** Sử dụng thông tin test ở trên
7. **Xác nhận đặt hàng**

## ⚠️ Lưu ý

- Đây là dữ liệu demo, không sử dụng trong production
- Mật khẩu được lưu plain text, trong thực tế cần hash
- Thông tin thẻ và ví là fake, chỉ để test
- Có thể cần thay đổi tên database trong script

## 🎯 Tính năng đã có sẵn

✅ Giao hàng tại nhà và nhận tại shop  
✅ Thanh toán tiền mặt, thẻ ngân hàng, MoMo  
✅ Validation đầy đủ  
✅ Giao diện responsive  
✅ Tính phí giao hàng tự động  
✅ Quản lý đơn hàng  
✅ Lịch sử giao dịch

Dự án đã hoàn thiện đầy đủ các chức năng bạn yêu cầu! 🎉
