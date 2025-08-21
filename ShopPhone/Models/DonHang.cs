namespace ShopPhone.Models
{
    public class DonHang
    {
        public bool DaXacNhan { get; set; } = false;

        public int DonHangId { get; set; }

        public int TaiKhoanId { get; set; }

        public string? TenDangNhap { get; set; }

        public DateTime NgayDat { get; set; }

        public decimal TongTien { get; set; }

        public string TrangThai { get; set; } = "Chờ xác nhận";

        // Thông tin thanh toán
        public int? PhuongThucThanhToanId { get; set; }

        public string? MaGiaoDich { get; set; }
        public string? TrangThaiThanhToan { get; set; } = "Chưa thanh toán";
        public DateTime? NgayThanhToan { get; set; }

        // Thông tin giao hàng
        public int? PhuongThucGiaoHangId { get; set; }

        public decimal PhiGiaoHang { get; set; } = 0;
        public decimal TongTienSauPhiGiaoHang { get; set; }

        // Navigation properties
        public PhuongThucThanhToan? PhuongThucThanhToan { get; set; }

        public PhuongThucGiaoHang? PhuongThucGiaoHang { get; set; }
        public ThongTinGiaoHang? ThongTinGiaoHang { get; set; }
        public List<ChiTietDonHang> ChiTietDonHang { get; set; } = new();
    }
}