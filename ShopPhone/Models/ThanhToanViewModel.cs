using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class ThanhToanViewModel
    {
        // Thông tin giao hàng
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [StringLength(100, ErrorMessage = "Họ tên không được quá 100 ký tự")]
        public string HoTen { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [StringLength(15, ErrorMessage = "Số điện thoại không được quá 15 ký tự")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ")]
        [StringLength(200, ErrorMessage = "Địa chỉ không được quá 200 ký tự")]
        public string DiaChi { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TinhThanh { get; set; }

        [StringLength(100)]
        public string? QuanHuyen { get; set; }

        [StringLength(500)]
        public string? GhiChu { get; set; }

        // Phương thức giao hàng
        [Required(ErrorMessage = "Vui lòng chọn phương thức giao hàng")]
        public int PhuongThucGiaoHangId { get; set; }

        // Phương thức thanh toán
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán")]
        public int PhuongThucThanhToanId { get; set; }

        // Thông tin thẻ tín dụng (chỉ khi chọn thanh toán bằng thẻ)
        [StringLength(16)]
        public string? SoThe { get; set; }

        [StringLength(50)]
        public string? ChuThe { get; set; }

        [StringLength(5)]
        public string? NgayHetHan { get; set; }

        [StringLength(4)]
        public string? CVV { get; set; }

        // Thông tin đơn hàng
        public decimal TongTien { get; set; }

        public decimal PhiGiaoHang { get; set; }
        public decimal TongTienSauPhiGiaoHang { get; set; }

        // Danh sách các phương thức
        public List<PhuongThucGiaoHang> DanhSachGiaoHang { get; set; } = new();

        public List<PhuongThucThanhToan> DanhSachThanhToan { get; set; } = new();
    }
}