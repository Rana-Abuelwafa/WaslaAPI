using System.ComponentModel.DataAnnotations;

namespace Wasla_Auth_App.Models
{
    public class AppsRegisterModel
    {
        [Required(ErrorMessage = "FirstName is required")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "LastName is required")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "email is required")]
        public required string Email { get; set; }

        public required string lang { get; set; }
        public string? Role { get; set; }

    }
}
