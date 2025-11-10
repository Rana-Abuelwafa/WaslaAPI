using System.ComponentModel.DataAnnotations;

namespace Wasla_Auth_App.Models
{
    public class AppsLoginModel
    {

        [Required(ErrorMessage = "email is required")]
        public required string Email { get; set; }

        public required string lang { get; set; }
  
    }
}
