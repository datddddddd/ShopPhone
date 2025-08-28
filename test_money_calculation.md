# 🧪 Test tính toán tiền sau khi sửa

## 📋 **Checklist test:**

### ✅ **1. Test cơ bản - Giao hàng tại nhà:**
```
Sản phẩm: iPhone 15 Demo - 25,000,000 VNĐ
Số lượng: 1
Phí giao hàng: 30,000 VNĐ
Tổng cộng: 25,030,000 VNĐ ✅
```

### ✅ **2. Test cơ bản - Giao hàng tại shop:**
```
Sản phẩm: iPhone 15 Demo - 25,000,000 VNĐ
Số lượng: 1
Phí giao hàng: 0 VNĐ
Tổng cộng: 25,000,000 VNĐ ✅
```

### ✅ **3. Test với giảm giá:**
```
Sản phẩm có giảm giá 10%:
- Giá gốc: 10,000,000 VNĐ
- Giảm 10%: -1,000,000 VNĐ
- Giá sau giảm: 9,000,000 VNĐ
- Phí giao hàng: 30,000 VNĐ
- Tổng cộng: 9,030,000 VNĐ ✅
```

### ✅ **4. Test với bảo hành:**
```
Sản phẩm + bảo hành:
- Giá sản phẩm: 10,000,000 VNĐ
- Bảo hành cơ bản: +990,000 VNĐ
- Tổng: 10,990,000 VNĐ
- Phí giao hàng: 30,000 VNĐ
- Tổng cộng: 11,020,000 VNĐ ✅
```

### ✅ **5. Test với nhiều sản phẩm:**
```
Sản phẩm 1: 20,000,000 VNĐ x 2 = 40,000,000 VNĐ
Sản phẩm 2: 15,000,000 VNĐ x 1 = 15,000,000 VNĐ
Tổng hàng: 55,000,000 VNĐ
Phí giao hàng: 30,000 VNĐ
Tổng cộng: 55,030,000 VNĐ ✅
```

## 🎯 **Các trang cần test:**

### **1. Trang chủ (/):**
- ✅ Hiển thị giá sản phẩm đúng format
- ✅ Nút "Thêm vào giỏ" hoạt động

### **2. Giỏ hàng (/GioHang):**
- ✅ Tính tổng tiền từng sản phẩm
- ✅ Tính tổng tiền toàn bộ giỏ hàng
- ✅ Cập nhật số lượng → tính lại tiền
- ✅ Thêm/bỏ bảo hành → tính lại tiền

### **3. Thanh toán (/ThanhToan):**
- ✅ Hiển thị tổng tiền hàng
- ✅ Chọn giao hàng tại nhà → +30,000 VNĐ
- ✅ Chọn giao hàng tại shop → +0 VNĐ
- ✅ Tổng cộng tính đúng
- ✅ Format tiền đúng (có dấu phẩy)

## 🔧 **Cách test thực tế:**

### **Bước 1: Chuẩn bị dữ liệu**
```sql
-- Chạy script tạo sản phẩm demo
-- Đảm bảo có sản phẩm với giá khác nhau
```

### **Bước 2: Test từng trang**

#### **Test Trang chủ:**
1. Truy cập `https://localhost:7010/`
2. Kiểm tra giá hiển thị: `25,000,000 VNĐ` (có dấu phẩy)
3. Click "Thêm vào giỏ" → thông báo thành công

#### **Test Giỏ hàng:**
1. Truy cập `https://localhost:7010/GioHang`
2. Kiểm tra tổng tiền hiển thị đúng
3. Thay đổi số lượng → kiểm tra tính lại
4. Thêm bảo hành → kiểm tra tính lại

#### **Test Thanh toán:**
1. Truy cập `https://localhost:7010/ThanhToan`
2. Kiểm tra "Tổng tiền hàng" hiển thị đúng
3. Chọn "Giao hàng tại nhà" → kiểm tra phí +30,000
4. Chọn "Giao hàng tại shop" → kiểm tra phí = 0
5. Kiểm tra "Tổng cộng" tính đúng

## 📱 **Test cases cụ thể:**

### **Case 1: Đơn hàng đơn giản**
```
Input:
- 1 x iPhone 15 Demo (25,000,000 VNĐ)
- Giao hàng tại nhà

Expected Output:
- Tổng tiền hàng: 25,000,000 VNĐ
- Phí giao hàng: 30,000 VNĐ
- Tổng cộng: 25,030,000 VNĐ
```

### **Case 2: Đơn hàng có giảm giá**
```
Input:
- 1 x Sản phẩm giảm 15% (20,000,000 VNĐ)
- Giao hàng tại shop

Expected Output:
- Giá gốc: 20,000,000 VNĐ
- Giảm 15%: -3,000,000 VNĐ
- Giá sau giảm: 17,000,000 VNĐ
- Phí giao hàng: 0 VNĐ
- Tổng cộng: 17,000,000 VNĐ
```

### **Case 3: Đơn hàng có bảo hành**
```
Input:
- 1 x Sản phẩm (15,000,000 VNĐ)
- Bảo hành cơ bản (+990,000 VNĐ)
- Giao hàng tại nhà

Expected Output:
- Giá sản phẩm: 15,000,000 VNĐ
- Bảo hành: +990,000 VNĐ
- Tổng hàng: 15,990,000 VNĐ
- Phí giao hàng: 30,000 VNĐ
- Tổng cộng: 16,020,000 VNĐ
```

### **Case 4: Đơn hàng phức tạp**
```
Input:
- 2 x iPhone (25,000,000 VNĐ) = 50,000,000 VNĐ
- 1 x Samsung giảm 10% (22,000,000 VNĐ) = 19,800,000 VNĐ
- Bảo hành premium (+1,300,000 VNĐ)
- Giao hàng tại nhà

Expected Output:
- Tổng hàng: 71,100,000 VNĐ
- Phí giao hàng: 30,000 VNĐ
- Tổng cộng: 71,130,000 VNĐ
```

## 🐛 **Các lỗi đã sửa:**

### **Trước khi sửa:**
- ❌ `20000000.430000.00 VNĐ`
- ❌ `NaN VNĐ`
- ❌ `undefined VNĐ`
- ❌ Lỗi `GiaGoc` property not found

### **Sau khi sửa:**
- ✅ `20,030,000 VNĐ`
- ✅ `0 VNĐ`
- ✅ `25,000,000 VNĐ`
- ✅ Sử dụng `DonGia` property

## 🎉 **Kết quả mong đợi:**

**✅ TẤT CẢ TÍNH TOÁN TIỀN HOẠT ĐỘNG ĐÚNG!**

- Format tiền: `25,030,000 VNĐ` (có dấu phẩy)
- Phí giao hàng: Tính đúng 30,000 hoặc 0
- Tổng cộng: Cộng đúng không bị lỗi
- JavaScript: Parse số đúng cách
- Backend: Sử dụng đúng property DonGia

**Người dùng sẽ thấy số tiền chính xác và dễ đọc!** 💰
