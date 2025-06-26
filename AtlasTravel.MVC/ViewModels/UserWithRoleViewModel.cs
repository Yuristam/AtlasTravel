using AtlasTravel.MVC.Models;

namespace AtlasTravel.MVC.ViewModels
{
    public class UserWithRoleViewModel
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }

        public List<Role> AllRoles { get; set; } = new();
    }
}
