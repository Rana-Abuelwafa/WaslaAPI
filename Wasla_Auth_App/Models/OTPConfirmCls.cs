using System.ComponentModel.DataAnnotations;

namespace Wasla_Auth_App.Models
{
    public class OTPConfirmCls
    {
        [Required(ErrorMessage = "email is required")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "otp is required")]
        public required string otp { get; set; }
    }
}
