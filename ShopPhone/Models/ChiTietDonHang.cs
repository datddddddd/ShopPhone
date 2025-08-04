using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPhone.Models
{
    public class ChiTietDonHang
    {
        [Key]  // ✅ Đánh dấu khóa chính cho EF
        public int Id { get; set; }

        public int DonHangId { get; set; }
        public int MaHH { get; set; }
        public int SoLuong { get; set; }
        public decimal? DonGia { get; set; }
        public decimal DonGiaGoc { get; set; }  // 👈 để lưu giá gốc chưa tính bảo hành

        public bool BaoHanh1 { get; set; }
        public bool BaoHanh2 { get; set; }
        public decimal GiamGia { get; set; }
        public decimal ThanhTien { get; set; }

        public DonHang DonHang { get; set; }    // Navigation property

        [ForeignKey("MaHH")]
        public HangHoa HangHoa { get; set; }    // Navigation property
    }
}