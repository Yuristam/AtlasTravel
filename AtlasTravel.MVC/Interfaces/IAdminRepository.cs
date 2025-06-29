using AtlasTravel.MVC.Models;
using AtlasTravel.MVC.ViewModels;

namespace AtlasTravel.MVC.Interfaces
{
    public interface IAdminRepository
    {
        Task<int> CountUsersAsync();
        // int CountTrips();
        //int CountFeedbacks();

        Task UpdateUserByAdminAsync(User user);
        Task<List<UserWithRoleViewModel>> GetAllUsersWithRolesAsync();
    }
}
