using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class ViDienTu
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string TenChuVi { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string SoDienThoai { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LoaiVi { get; set; } = "Momo";

        public int TaiKhoanId { get; set; } // Chủ ví
    }
}
