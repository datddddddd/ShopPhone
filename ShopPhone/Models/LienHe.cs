using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class LienHe
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string HoTen { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập nội dung")]
        public string NoiDung { get; set; }

        public DateTime NgayGui { get; set; } = DateTime.Now;
        public string? UserId { get; set; }
    }
}