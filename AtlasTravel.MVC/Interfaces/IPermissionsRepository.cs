using AtlasTravel.MVC.ViewModels;

namespace AtlasTravel.MVC.Interfaces
{
    public interface IPermissionsRepository
    {
        Task ClearPermissionsAsync(int roleId);
        Task AssignPermissionToRoleAsync(int roleId, int permissionId);
        Task<ManageRolePermissionsViewModel> GetRolePermissionsAsync(int roleId);
    }
}
