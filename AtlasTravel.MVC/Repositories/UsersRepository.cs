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

            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users";
            using var command = new SqlCommand(query, connection);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                var user = new User
                {
                    UserID = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3),
                    Budget = reader.GetDecimal(4)
                };

                users.Add(user);
            }

            return users;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "SELECT * FROM Users";
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ID", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

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
            using var connection = new SqlConnection(_connectionString);
            var query = @"INSERT INTO Users (FullName, Email, Password, Budget)
                          VALUES (@FullName, @Email, @Password, @Budget)";
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Budget", user.Budget);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = @"UPDATE Users
                          SET FullName = @FullName, Email = @Email, Password = @Password, Budget = @Budget
                          WHERE UserID = @UserID";
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@Password", user.Password);
            command.Parameters.AddWithValue("@Budget", user.Budget);
            command.Parameters.AddWithValue("@UserID", user.UserID);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }

        public async Task DeleteUserAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            var query = "DELETE FROM Users WHERE UserID = @UserID";
            using var command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@UserID", id);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
}
