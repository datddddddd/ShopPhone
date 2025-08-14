namespace ShopPhone.Models
{
    public class GioHangDb
    {
        public int Id { get; set; }

        public string? MaNguoiDung { get; set; }

        public DateTime NgayTao { get; set; }

        public string? TenDangNhap { get; set; }

        // Navigation property nếu có (không bắt buộc)
        public ICollection<GioHangChiTietDb> ChiTietGioHang { get; set; } = new List<GioHangChiTietDb>();
    }
}