using System.ComponentModel.DataAnnotations;

namespace Wasla_Auth_App.Models
{
    public class RegisterModel
    {
        //[Required(ErrorMessage = "username is required")]
        //public required string Username { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "email is required")]
        public required string Email { get; set; }


        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; set; }

        [Compare("Password", ErrorMessage = "Confirm password doesn't match, Type again !")]
        public string? ConfirmPassword { get; set; }
    }
}
