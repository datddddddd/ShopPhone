using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class PhuongThucGiaoHang
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Ten { get; set; } = string.Empty;

        [StringLength(500)]
        public string? MoTa { get; set; }

        public decimal PhiGiaoHang { get; set; } = 0;

        public bool HoatDong { get; set; } = true;

        public int ThuTu { get; set; } = 0;
    }
}