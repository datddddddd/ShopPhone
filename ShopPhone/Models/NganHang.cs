using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class NganHang
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenNganHang { get; set; } = string.Empty;

        [StringLength(10)]
        public string MaNganHang { get; set; } = string.Empty;

        [StringLength(200)]
        public string Logo { get; set; } = string.Empty;

        [StringLength(50)]
        public string LoaiThe { get; set; } = string.Empty; // Visa, MasterCard, JCB, etc.

        [StringLength(20)]
        public string MauThe { get; set; } = string.Empty; // Màu chủ đạo của thẻ

        [StringLength(500)]
        public string MoTa { get; set; } = string.Empty;

        public bool HoatDong { get; set; } = true;

        public int ThuTu { get; set; } = 0;

        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}