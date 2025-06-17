using AtlasTravel.MVC.Helpers;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly string _connectionString;

        public UsersRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<ICollection<User>> GetAllUsersAsync()
        {
            var users = new List<User>();

            var sql = "SELECT * FROM Users";

            using var reader = await SqlHelper.ExecuteReaderAsync(_connectionString, sql);

            while (await reader.ReadAsync())
            {
                users.Add(new User
                {
                    UserID = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3),
                    Budget = reader.GetDecimal(4)
                });
            }

            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var sql = "SELECT * FROM Users WHERE UserID = @ID";
            var parameters = new[] { new SqlParameter("@ID", id) };

            using var reader = await SqlHelper.ExecuteReaderAsync(_connectionString, sql, parameters);

            if (await reader.ReadAsync())
            {
                return new User
                {
                    UserID = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3),
                    Budget = reader.GetDecimal(4)
                };
            }

            return null;
        }

        public async Task CreateUserAsync(User user)
        {
            var sql = @"INSERT INTO Users (FullName, Email, Password, Budget)
                        VALUES (@FullName, @Email, @Password, @Budget)";

            var parameters = new[] {
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@Budget", user.Budget)
            };

            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public async Task UpdateUserAsync(User user)
        {
            var sql = @"UPDATE Users
                        SET FullName = @FullName, Password = @Password, Budget = @Budget
                        WHERE UserID = @UserID";

            var parameters = new[] { 
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@Budget", user.Budget),
                new SqlParameter("@UserID", user.UserID)
            };

            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public async Task DeleteUserAsync(int id)
        {
            var sql = "DELETE FROM Users WHERE UserID = @UserID";
            var parameters = new[] { new SqlParameter("@UserID", id) };
            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }
    }
}
