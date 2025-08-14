using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class TheTinDung
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16)]
        public string SoThe { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string ChuThe { get; set; } = string.Empty;

        [Required]
        [StringLength(5)]
        public string NgayHetHan { get; set; } = string.Empty; // Format: MM/YY

        [Required]
        [StringLength(4)]
        public string CVV { get; set; } = string.Empty;

        public string LoaiThe { get; set; } = "Visa"; // Visa, MasterCard, etc.

        public decimal HanMuc { get; set; } = decimal.MaxValue; // Vô hạn cho test

        public bool HoatDong { get; set; } = true;

        public DateTime NgayTao { get; set; } = new DateTime(2024, 1, 1);
    }
}