using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Views
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải ít nhất 6 ký tự.")]
        public string Password { get; set; }
    }
}