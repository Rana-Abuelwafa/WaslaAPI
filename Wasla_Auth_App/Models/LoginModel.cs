using System.ComponentModel.DataAnnotations;

namespace Wasla_Auth_App.Models
{
    public class LoginModel
    {
        

        [Required(ErrorMessage = "email is required")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }
    }
}
