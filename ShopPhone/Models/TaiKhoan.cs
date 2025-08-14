using System.ComponentModel.DataAnnotations.Schema;

namespace ShopPhone.Models
{
    public class TaiKhoan
    {
        public int Id { get; set; }

        public string MatKhau { get; set; } = null!;
        public string VaiTro { get; set; } = "User";
        public string Email { get; set; } = null!;
        public DateTime NgayTao { get; set; } = DateTime.Now;
        public string HoTen { get; set; } = null!;

        [NotMapped]
        public IFormFile? FileAnhDaiDien { get; set; }

        public string? AnhDaiDien { get; set; }
    }
}