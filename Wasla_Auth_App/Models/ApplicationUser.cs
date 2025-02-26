using Microsoft.AspNetCore.Identity;

namespace Wasla_Auth_App.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName {  get; set; }
        public string? LastName { get; set; }
        public string? CloudId { get; set; }
        public string? GoogleId { get; set; }
        public string? FaceBookId { get; set; }
    }
}
