# 🔍 Kiểm tra và sửa lỗi hình ảnh ngân hàng

## ✅ **Đã sửa các vấn đề:**

### 1. **📁 Đường dẫn file không khớp**

**Vấn đề:** Tên file trong database khác với file thực tế trong folder Banks

**Files thực tế trong `/wwwroot/Banks/`:**
```
✅ vietcombank.jpg      (không phải .png)
✅ Techcombank.png      (viết hoa T)
✅ Bidv.jpg             (viết hoa B, không phải .png)
✅ VietinBank.png       (viết hoa V)
✅ Agribank.png         (viết hoa A)
✅ Acb.png              (viết hoa A)
✅ sacombank.png        (viết thường)
✅ VPBank.png           (viết hoa VP)
✅ MBBank.jpg           (viết hoa MB, không phải .png)
✅ TPBank.webp          (viết hoa TP, định dạng .webp)
✅ HSBC.png             (viết hoa toàn bộ)
✅ standardchartered.jpg (viết thường, không phải .png)
✅ citibank.png         (viết thường)
✅ shinhanbank.png      (viết thường)
✅ ANZBank.png          (viết hoa ANZ)
```

### 2. **🔧 Đã cập nhật SQL script**

**File:** `add_banks_data.sql`

**Trước:**
```sql
('/Banks/vietcombank.png', ...)  -- ❌ Sai
('/Banks/techcombank.png', ...)  -- ❌ Sai
('/Banks/bidv.png', ...)         -- ❌ Sai
```

**Sau:**
```sql
('/Banks/vietcombank.jpg', ...)  -- ✅ Đúng
('/Banks/Techcombank.png', ...)  -- ✅ Đúng
('/Banks/Bidv.jpg', ...)         -- ✅ Đúng
```

### 3. **🎨 Đã cập nhật View**

**File:** `ThanhToanTheTinDung.cshtml`

**Thêm fallback thông minh:**
```html
<img src="@bank.Logo" alt="@bank.TenNganHang" 
     style="max-width: 60px; max-height: 40px; object-fit: contain;"
     onerror="this.style.display='none'; this.nextElementSibling.style.display='block';" />
<i class="fas fa-university" style="display: none; font-size: 2rem; color: #667eea;"></i>
```

**Tính năng:**
- ✅ **Auto-resize:** Hình ảnh tự động resize về 60x40px
- ✅ **Object-fit:** Giữ tỷ lệ hình ảnh
- ✅ **Error handling:** Nếu hình lỗi → hiện icon ngân hàng
- ✅ **Consistent styling:** Icon màu tím nhất quán

### 4. **🔄 Cập nhật default banks trong view**

**Sửa 3 ngân hàng mặc định:**
```html
<!-- Trước -->
<img src="/Banks/vietcombank.png" ... />  <!-- ❌ -->
<img src="/Banks/techcombank.png" ... />  <!-- ❌ -->
<img src="/Banks/bidv.png" ... />         <!-- ❌ -->

<!-- Sau -->
<img src="/Banks/vietcombank.jpg" ... />  <!-- ✅ -->
<img src="/Banks/Techcombank.png" ... />  <!-- ✅ -->
<img src="/Banks/Bidv.jpg" ... />         <!-- ✅ -->
```

## 🚀 **Cách test:**

### **1. Chạy SQL script để cập nhật database:**
```sql
-- Chạy file add_banks_data.sql để cập nhật đường dẫn hình ảnh
```

### **2. Build và test:**
```bash
dotnet build
dotnet run
```

### **3. Kiểm tra hình ảnh:**
1. **Vào trang thanh toán thẻ tín dụng**
2. **Kiểm tra logo ngân hàng:**
   - ✅ Vietcombank → Hiện logo .jpg
   - ✅ Techcombank → Hiện logo .png
   - ✅ BIDV → Hiện logo .jpg
   - ✅ Các ngân hàng khác → Hiện logo tương ứng
3. **Nếu hình lỗi → Hiện icon ngân hàng màu tím**

### **4. Test fallback:**
```html
<!-- Để test fallback, tạm thời sửa đường dẫn sai -->
<img src="/Banks/test-wrong-path.png" ... />
<!-- Kết quả: Hiện icon ngân hàng thay vì hình lỗi -->
```

## 📋 **Checklist hoàn thành:**

- ✅ **Tạo folder Banks trong wwwroot**
- ✅ **Copy 15 file hình ảnh ngân hàng**
- ✅ **Cập nhật SQL script với đường dẫn đúng**
- ✅ **Sửa view với fallback thông minh**
- ✅ **Cập nhật 3 ngân hàng mặc định**
- ✅ **Thêm error handling cho hình ảnh**
- ✅ **Styling nhất quán với theme tím**

## 🎯 **Kết quả mong đợi:**

### **✅ Trước khi sửa:**
- ❌ Hình ảnh không hiển thị
- ❌ Chỉ có icon generic
- ❌ Đường dẫn file sai

### **✅ Sau khi sửa:**
- ✅ **15 logo ngân hàng hiển thị đẹp**
- ✅ **Fallback thông minh** nếu hình lỗi
- ✅ **Responsive design** - resize tự động
- ✅ **Professional look** - chuyên nghiệp

## 🔧 **Nếu vẫn không hiển thị:**

### **1. Kiểm tra đường dẫn:**
```
Đảm bảo files nằm đúng vị trí:
📁 ShopPhone/
  📁 wwwroot/
    📁 Banks/
      📄 vietcombank.jpg
      📄 Techcombank.png
      📄 Bidv.jpg
      📄 ... (12 files khác)
```

### **2. Kiểm tra database:**
```sql
-- Kiểm tra dữ liệu ngân hàng
SELECT TenNganHang, Logo FROM NganHang ORDER BY ThuTu;

-- Kết quả mong đợi:
-- Vietcombank | /Banks/vietcombank.jpg
-- Techcombank | /Banks/Techcombank.png
-- BIDV        | /Banks/Bidv.jpg
```

### **3. Kiểm tra browser:**
```
1. F12 → Network tab
2. Reload trang
3. Kiểm tra request đến /Banks/xxx.png
4. Nếu 404 → File không tồn tại
5. Nếu 200 → File OK, có thể do CSS
```

### **4. Clear cache:**
```
Ctrl + F5 để hard refresh
Hoặc clear browser cache
```

## 🎉 **Kết luận:**

**Bây giờ hình ảnh ngân hàng sẽ hiển thị đẹp với:**
- ✅ **15 logo ngân hàng chính xác**
- ✅ **Fallback icon nếu lỗi**
- ✅ **Responsive và professional**
- ✅ **Nhất quán với theme tím**

**Trang thanh toán thẻ tín dụng giờ đã hoàn hảo!** 💳✨
