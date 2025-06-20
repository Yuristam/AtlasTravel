using AtlasTravel.MVC.Models;

namespace AtlasTravel.MVC.Interfaces
{
    public interface IAdminRepository
    {
        Task<int> CountUsersAsync();
        // int CountTrips();
        //int CountFeedbacks();

        Task UpdateUserByAdminAsync(User user);
    }
}
