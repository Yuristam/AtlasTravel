using AtlasTravel.MVC.Models;

namespace AtlasTravel.MVC.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserByIdAsync(int id);
        Task<ICollection<User>> GetAllUsersAsync();
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
    }
}
