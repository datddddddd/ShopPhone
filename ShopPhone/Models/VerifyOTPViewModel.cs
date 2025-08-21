using System.ComponentModel.DataAnnotations;

namespace ShopPhone.Models
{
    public class VerifyOTPViewModel
    {
        [Required]
        public string OTP { get; set; }
    }
}