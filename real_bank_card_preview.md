# 💳 Hiển thị Thẻ Ngân Hàng Thật trên Preview

## 🎯 **Tính năng mới:**

### **Khi chọn Vietcombank:**
```
💳 [Thẻ Preview]
   🖼️ Background: Hình ảnh thẻ Vietcombank thật (vietcombank.jpg)
   📝 Text overlay: Số thẻ, tên, ngày hết hạn người dùng nhập
   🎨 Trông giống hệt thẻ Vietcombank thật
```

### **Khi chọn Techcombank:**
```
💳 [Thẻ Preview]
   🖼️ Background: Hình ảnh thẻ Techcombank thật (Techcombank.png)
   📝 Text overlay: Thông tin người dùng
   🎨 Trông giống hệt thẻ Techcombank thật
```

## 🔧 **Cách hoạt động:**

### **1. Cấu trúc HTML mới:**

**Trước:**
```html
<div class="credit-card-preview">
    <div class="card-number">**** **** **** ****</div>
    <!-- Chỉ có text trên nền gradient -->
</div>
```

**Sau:**
```html
<div class="credit-card-preview">
    <!-- Default card (khi chưa chọn ngân hàng) -->
    <div class="card-content" id="defaultCardContent">
        <div class="card-number">**** **** **** ****</div>
        <!-- Text trên nền gradient tím -->
    </div>
    
    <!-- Bank card overlay (khi đã chọn ngân hàng) -->
    <div class="bank-card-overlay" id="bankCardOverlay" style="display: none;">
        <!-- Background: Hình ảnh thẻ ngân hàng thật -->
        <div class="card-overlay-content">
            <!-- Text overlay màu trắng với shadow -->
            <div id="cardNumberOverlay">**** **** **** ****</div>
            <div id="cardHolderOverlay">YOUR NAME</div>
            <div id="cardExpiryOverlay">MM/YY</div>
        </div>
    </div>
</div>
```

### **2. JavaScript Logic:**

```javascript
function updateCardPreview() {
    if (selectedBank && selectedBank.logo) {
        // ✅ Hiển thị hình ảnh thẻ ngân hàng thật
        const bankCardOverlay = $('#bankCardOverlay');
        const defaultCardContent = $('#defaultCardContent');
        
        // Ẩn thẻ mặc định, hiện thẻ ngân hàng
        defaultCardContent.hide();
        bankCardOverlay.show();
        bankCardOverlay.css('background-image', `url('${selectedBank.logo}')`);
        
        // Cập nhật text overlay
        updateOverlayText();
    } else {
        // ✅ Hiển thị thẻ mặc định
        bankCardOverlay.hide();
        defaultCardContent.show();
    }
}

function updateOverlayText() {
    // Đồng bộ text overlay với input của user
    const cardNumber = $('#SoThe').val() || '**** **** **** ****';
    const cardHolder = $('#ChuThe').val() || 'YOUR NAME';
    const cardExpiry = $('#NgayHetHan').val() || 'MM/YY';
    
    $('#cardNumberOverlay').text(cardNumber);
    $('#cardHolderOverlay').text(cardHolder.toUpperCase());
    $('#cardExpiryOverlay').text(cardExpiry);
}
```

### **3. Đồng bộ Input Events:**

```javascript
// Khi user nhập số thẻ
$('#SoThe').on('input', function() {
    const value = $(this).val();
    $('#cardNumberPreview').text(value);      // Default card
    $('#cardNumberOverlay').text(value);      // Bank card overlay
});

// Khi user nhập tên
$('#ChuThe').on('input', function() {
    const value = $(this).val().toUpperCase();
    $('#cardHolderPreview').text(value);      // Default card
    $('#cardHolderOverlay').text(value);      // Bank card overlay
});

// Khi user nhập ngày hết hạn
$('#NgayHetHan').on('input', function() {
    const value = $(this).val();
    $('#cardExpiryPreview').text(value);      // Default card
    $('#cardExpiryOverlay').text(value);      // Bank card overlay
});
```

## 🎨 **Styling đặc biệt:**

### **1. Text Overlay:**
```css
color: white;
text-shadow: 1px 1px 2px rgba(0,0,0,0.5);
```
- ✅ **Màu trắng** nổi bật trên mọi nền
- ✅ **Text shadow** để đọc rõ trên nền phức tạp

### **2. Background Image:**
```css
background-size: cover;
background-position: center;
border-radius: 20px;
```
- ✅ **Cover** để hình ảnh phủ kín thẻ
- ✅ **Center** để logo ngân hàng ở giữa
- ✅ **Border radius** giống thẻ thật

### **3. Layout:**
```css
position: absolute;
top: 0; left: 0;
width: 100%; height: 100%;
display: flex;
flex-direction: column;
justify-content: space-between;
padding: 20px;
```
- ✅ **Absolute positioning** để overlay chính xác
- ✅ **Flexbox** để căn chỉnh text như thẻ thật

## 🚀 **Demo Flow:**

### **1. Trạng thái ban đầu:**
```
💳 [Thẻ Preview]
   🎨 Nền gradient tím mặc định
   📝 **** **** **** ****
   👤 YOUR NAME
   📅 MM/YY
```

### **2. User chọn Vietcombank:**
```
💳 [Thẻ Preview]
   🖼️ Background: Hình ảnh thẻ Vietcombank thật
   📝 **** **** **** **** (màu trắng với shadow)
   👤 YOUR NAME (màu trắng với shadow)
   📅 MM/YY (màu trắng với shadow)
```

### **3. User nhập thông tin:**
```
💳 [Thẻ Preview]
   🖼️ Background: Hình ảnh thẻ Vietcombank thật
   📝 4111 1111 1111 1111 (cập nhật real-time)
   👤 NGUYEN VAN A (cập nhật real-time)
   📅 12/25 (cập nhật real-time)
```

### **4. User chọn ngân hàng khác:**
```
💳 [Thẻ Preview]
   🖼️ Background: Hình ảnh thẻ Techcombank thật
   📝 4111 1111 1111 1111 (giữ nguyên thông tin)
   👤 NGUYEN VAN A (giữ nguyên thông tin)
   📅 12/25 (giữ nguyên thông tin)
```

## ✅ **Lợi ích:**

### **1. Trải nghiệm người dùng:**
- ✅ **Visual feedback tức thời** khi chọn ngân hàng
- ✅ **Realistic preview** giống thẻ thật
- ✅ **Professional look** tăng độ tin cậy

### **2. Tính năng kỹ thuật:**
- ✅ **Real-time sync** giữa input và preview
- ✅ **Fallback graceful** khi không có logo
- ✅ **Responsive design** trên mọi thiết bị

### **3. Business value:**
- ✅ **Tăng conversion rate** do UX tốt hơn
- ✅ **Giảm confusion** về ngân hàng được chọn
- ✅ **Brand recognition** cho các ngân hàng

## 🔧 **Test Cases:**

### **1. Test hiển thị thẻ:**
```
1. Vào trang thanh toán
2. ✅ Kiểm tra: Thẻ mặc định gradient tím
3. Click chọn Vietcombank
4. ✅ Kiểm tra: Thẻ hiển thị hình ảnh Vietcombank
5. Click chọn Techcombank
6. ✅ Kiểm tra: Thẻ chuyển sang hình ảnh Techcombank
```

### **2. Test input sync:**
```
1. Chọn Vietcombank
2. Nhập số thẻ: 4111 1111 1111 1111
3. ✅ Kiểm tra: Text hiện trên thẻ Vietcombank
4. Nhập tên: Nguyen Van A
5. ✅ Kiểm tra: Tên hiện trên thẻ Vietcombank
6. Nhập ngày: 12/25
7. ✅ Kiểm tra: Ngày hiện trên thẻ Vietcombank
```

### **3. Test fallback:**
```
1. Chọn ngân hàng không có logo
2. ✅ Kiểm tra: Hiển thị thẻ mặc định
3. Sửa đường dẫn logo sai
4. ✅ Kiểm tra: Fallback về thẻ mặc định
```

## 🎉 **Kết quả:**

**Bây giờ khi bạn chọn Vietcombank, thẻ preview sẽ:**

- ✅ **Hiển thị chính hình ảnh thẻ Vietcombank thật**
- ✅ **Text overlay màu trắng với shadow**
- ✅ **Cập nhật real-time khi nhập thông tin**
- ✅ **Trông giống hệt thẻ Vietcombank thật**

**Trải nghiệm giờ đã hoàn hảo như bạn mong muốn!** 💳✨
