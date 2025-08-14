using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class Login
    {
        [Required(ErrorMessage = "Vui lòng nhập email hoặc số điện thoại")]
        [Display(Name = "Số điện thoại hoặc Email")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
    }
}