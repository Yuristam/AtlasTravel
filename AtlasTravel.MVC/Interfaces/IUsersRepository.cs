using AtlasTravel.MVC.Models;

namespace AtlasTravel.MVC.Interfaces
{
    public interface IUsersRepository
    {
        Task<ICollection<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByEmailAsync(string email);
        Task CreateUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);
        Task ChangePassword(int userId, string newPassword);
    }
}
