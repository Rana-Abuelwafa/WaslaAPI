using System.ComponentModel.DataAnnotations;

namespace Wasla_Auth_App.Models
{
    public class ModelCls
    {
        [Required(ErrorMessage = "email is required")]
        public required string Email { get; set; }

    }
}
