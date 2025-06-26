using AtlasTravel.MVC.Models;

namespace AtlasTravel.MVC.ViewModels
{
    public class ManageRolePermissionsViewModel
    {
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public List<Permission> AllPermissions { get; set; } = new();
        public List<int> AssignedPermissions { get; set; } = new();
    }

}
