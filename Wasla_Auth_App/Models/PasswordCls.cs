using System.ComponentModel.DataAnnotations;

namespace Wasla_Auth_App.Models
{
    public class PasswordCls
    {
        [Required(ErrorMessage = "userId is required")]
        public string? userId { get; set; }
        [Required(ErrorMessage = "OldPassword is required")]
        public required string OldPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "New Password is required !")]
        public string? NewPassword { get; set; }
        [Compare("NewPassword", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string? ConfirmNewPassword { get; set; }
    }
}
