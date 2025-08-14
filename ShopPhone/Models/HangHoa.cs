using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPhone.Models
{
    public class HangHoa
    {
        [Key]
        public int MaHH { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string TenHH { get; set; }

        public string? TenAlias { get; set; }
        public int MaLoai { get; set; }
        public string MoTaDonVi { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải >= 0")]
        public decimal? DonGia { get; set; }    // giá đã giảm

        public decimal? GiamGia { get; set; }    // % giảm
        public string? Hinh { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn ngày sản xuất")]
        [DataType(DataType.Date)]
        public DateTime NgaySX { get; set; }

        public int? SoLanXem { get; set; }
        public string? MoTa { get; set; }
        public string? MaNCC { get; set; }
        public double? DanhGia { get; set; }
        public string? HinhMoHop { get; set; }
        public string? HinhThucTe { get; set; }
        public string? VideoId { get; set; }

        /* ---------- Thuộc tính tính toán ---------- */

        [NotMapped]
        public IFormFile? FileHinh { get; set; }

        [NotMapped]
        public IFormFile? FileHinhMoHop { get; set; }

        [NotMapped]
        public IFormFile? FileHinhThucTe { get; set; }

        /// <summary>Giá gốc suy ngược từ DonGia và % giảm.</summary>
        [NotMapped]
        public decimal GiaGoc =>
            (GiamGia ?? 0) > 0 && DonGia.HasValue
                ? Math.Round(DonGia.Value / (1 - (GiamGia.Value / 100m)))
                : DonGia.GetValueOrDefault();

        /// <summary>Giá bán hiện tại (đã giảm) – dùng luôn DonGia nếu có.</summary>
        [NotMapped]
        public decimal GiaKhuyenMai =>
            DonGia.HasValue
                ? Math.Round(DonGia.Value)
                : GiaGoc;   // fallback
    }
}