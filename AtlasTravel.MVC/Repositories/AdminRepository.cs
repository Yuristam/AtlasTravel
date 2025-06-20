using AtlasTravel.MVC.Helpers;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString;

        public AdminRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<int> CountUsersAsync()
        {
            var sql = "SELECT COUNT(*) FROM Users";
            var result = await SqlHelper.ExecuteScalarAsync(_connectionString, sql);

            return Convert.ToInt32(result);
        }

        public async Task UpdateUserByAdminAsync(User user)
        {
            var sql = @"UPDATE Users
                        SET FullName = @FullName, Budget = @Budget, Password = @Password
                        WHERE UserID = @UserID";

            var parameters = new[] {
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Budget", (object?)user.Budget ?? DBNull.Value),
                new SqlParameter("@Password", user.Password),
                new SqlParameter("@UserID", user.UserID)
            };

            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }
    }
}
