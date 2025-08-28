# 🔧 Sửa lỗi tính toán tiền trong Shop Phone

## 🚨 **Vấn đề đã phát hiện:**

### 1. **Lỗi chính:**
- **ThanhToanController** sử dụng `hangHoa.GiaGoc` nhưng model HangHoa không có property này
- **JavaScript** trong view tính toán phí giao hàng sai format
- **Kết quả:** Tổng cộng hiển thị `20000000.430000.00 VNĐ` thay vì `20,030,000 VNĐ`

### 2. **Nguyên nhân:**
- Model HangHoa chỉ có `DonGia`, không có `GiaGoc`
- JavaScript không parse số đúng cách
- Không sử dụng locale formatting cho tiền Việt Nam

## ✅ **Đã sửa các file:**

### 1. **ThanhToanController.cs:**
```csharp
// Trước (lỗi):
decimal giaGoc = hangHoa.GiaGoc;

// Sau (đã sửa):
decimal giaGoc = hangHoa.DonGia ?? 0;
```

### 2. **ThanhToan/Index.cshtml JavaScript:**
```javascript
// Trước (lỗi):
var phiGiaoHang = $(this).data('phi');
$('#tongCong').text(tongCong.toLocaleString() + ' VNĐ');

// Sau (đã sửa):
var phiGiaoHang = parseFloat($(this).data('phi')) || 0;
$('#tongCong').text(tongCong.toLocaleString('vi-VN') + ' VNĐ');
```

## 🔧 **Chi tiết các thay đổi:**

### **File: ShopPhone/Controllers/ThanhToanController.cs**

#### **Sửa 3 chỗ tính toán:**

1. **Tính tổng tiền (dòng 39-54):**
   - Đổi `hangHoa.GiaGoc` → `hangHoa.DonGia ?? 0`

2. **Tính tổng tiền trong XuLyThanhToan (dòng 106-121):**
   - Đổi `hangHoa.GiaGoc` → `hangHoa.DonGia ?? 0`

3. **Tạo chi tiết đơn hàng (dòng 196-209):**
   - Đổi `hangHoa.GiaGoc` → `hangHoa.DonGia ?? 0`

### **File: ShopPhone/Views/ThanhToan/Index.cshtml**

#### **Sửa JavaScript tính phí giao hàng:**
```javascript
$('input[name="PhuongThucGiaoHangId"]').change(function () {
    var phiGiaoHang = parseFloat($(this).data('phi')) || 0;
    var tongTien = @Model.TongTien;
    var tongCong = tongTien + phiGiaoHang;

    $('#phiGiaoHang').text(phiGiaoHang.toLocaleString('vi-VN') + ' VNĐ');
    $('#tongCong').text(tongCong.toLocaleString('vi-VN') + ' VNĐ');
});
```

## 🎯 **Kết quả sau khi sửa:**

### **Trước khi sửa:**
- Tổng tiền hàng: 20,000,000 VNĐ
- Phí giao hàng: 30,000 VNĐ
- **Tổng cộng: 20000000.430000.00 VNĐ** ❌

### **Sau khi sửa:**
- Tổng tiền hàng: 20,000,000 VNĐ
- Phí giao hàng: 30,000 VNĐ
- **Tổng cộng: 20,030,000 VNĐ** ✅

## 🧪 **Cách test:**

### 1. **Test tính toán cơ bản:**
```
1. Thêm sản phẩm 20,000,000 VNĐ vào giỏ
2. Vào trang thanh toán
3. Chọn "Giao hàng tại nhà" (30,000 VNĐ)
4. Kiểm tra tổng cộng = 20,030,000 VNĐ
```

### 2. **Test với giảm giá:**
```
1. Thêm sản phẩm có giảm giá 10%
2. Giá gốc: 10,000,000 VNĐ
3. Giá sau giảm: 9,000,000 VNĐ
4. Phí giao hàng: 30,000 VNĐ
5. Tổng cộng: 9,030,000 VNĐ
```

### 3. **Test với bảo hành:**
```
1. Thêm sản phẩm + bảo hành 990,000 VNĐ
2. Giá sản phẩm: 10,000,000 VNĐ
3. Tổng: 10,990,000 VNĐ
4. Phí giao hàng: 30,000 VNĐ
5. Tổng cộng: 11,020,000 VNĐ
```

## 🔍 **Các chỗ tính toán tiền khác đã kiểm tra:**

### ✅ **GioHangController.cs:**
- `CapNhatSoLuong`: Sử dụng đúng `HangHoa.DonGia`
- `ThemVaoGio`: Sử dụng đúng `hang.DonGia ?? 0`
- Không cần sửa

### ✅ **GioHang/Index.cshtml:**
- JavaScript `formatVN()` đã đúng
- Sử dụng `toLocaleString('vi-VN')`
- Không cần sửa

### ✅ **Home/Index.cshtml:**
- Hiển thị giá sử dụng `@sp.DonGia?.ToString("N0")`
- Đã đúng format
- Không cần sửa

## 🎨 **Cải thiện thêm:**

### **Format tiền tệ nhất quán:**
```javascript
// Sử dụng function chung cho format tiền
function formatMoney(amount) {
    return amount.toLocaleString('vi-VN') + ' VNĐ';
}

// Áp dụng:
$('#tongCong').text(formatMoney(tongCong));
$('#phiGiaoHang').text(formatMoney(phiGiaoHang));
```

### **Validation số tiền:**
```javascript
// Đảm bảo số không âm
var phiGiaoHang = Math.max(0, parseFloat($(this).data('phi')) || 0);
```

## ⚠️ **Lưu ý quan trọng:**

1. **Model HangHoa:** Chỉ có `DonGia`, không có `GiaGoc`
2. **Decimal precision:** Sử dụng `decimal(18,2)` cho tiền tệ
3. **JavaScript:** Luôn parse số trước khi tính toán
4. **Locale:** Sử dụng `'vi-VN'` cho format tiền Việt Nam
5. **Null safety:** Luôn check `?? 0` cho decimal nullable

## 🎉 **Kết quả:**

**✅ TẤT CẢ LỖI TÍNH TOÁN TIỀN ĐÃ ĐƯỢC SỬA!**

- ❌ `20000000.430000.00 VNĐ` → ✅ `20,030,000 VNĐ`
- ❌ Lỗi `GiaGoc` property → ✅ Sử dụng `DonGia`
- ❌ JavaScript format sai → ✅ Sử dụng `toLocaleString('vi-VN')`

**Bây giờ tất cả tính toán tiền sẽ hiển thị đúng!** 🚀
