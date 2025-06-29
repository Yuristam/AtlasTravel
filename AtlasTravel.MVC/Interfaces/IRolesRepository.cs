using AtlasTravel.MVC.Models;
using AtlasTravel.MVC.ViewModels;

namespace AtlasTravel.MVC.Interfaces
{
    public interface IRolesRepository
    {
        Task<List<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(int roleId);
        Task<List<UserWithRoleViewModel>> GetUsersByRoleAsync(int roleId);
        Task CreateRoleAsync(string role);
        Task DeleteRoleAsync(int roleId);
        Task AssignRoleToUserAsync(int userId, int roleId);
    }
}
