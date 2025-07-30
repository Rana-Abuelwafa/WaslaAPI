namespace Wasla_Auth_App.Models
{
    public class UsersCls
    {
        public bool? success { get; set; }
        public List<UsersWithRoles>? users { get; set; }
    }

    public class UsersResponse
    {
        public bool? success { get; set; }
        public List<UsersWithRolesGrp>? result { get; set; }
    }
}
