using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class ThongTinGiaoHang
    {
        public int Id { get; set; }

        public int DonHangId { get; set; }

        [Required]
        [StringLength(100)]
        public string HoTen { get; set; } = string.Empty;

        [Required]
        [StringLength(15)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string DiaChi { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TinhThanh { get; set; }

        [StringLength(100)]
        public string? QuanHuyen { get; set; }

        [StringLength(500)]
        public string? GhiChu { get; set; }

        public DateTime NgayGiaoDuKien { get; set; }

        public string? MaVanDon { get; set; }

        public string TrangThaiGiaoHang { get; set; } = "Chờ xử lý";

        // Navigation property
        public DonHang? DonHang { get; set; }
    }
}