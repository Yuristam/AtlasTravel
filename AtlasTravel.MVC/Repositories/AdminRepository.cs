using AtlasTravel.MVC.Helpers;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using AtlasTravel.MVC.ViewModels;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly string _connectionString;
        private readonly IRolesRepository _rolesRepository;

        public AdminRepository(string connectionString, IRolesRepository rolesRepository)
        {
            _connectionString = connectionString;
            _rolesRepository = rolesRepository;
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

        public async Task<List<UserWithRoleViewModel>> GetAllUsersWithRolesAsync()
        {
            var users = new List<UserWithRoleViewModel>();
            var roles = await _rolesRepository.GetAllRolesAsync();

            var sql = @"SELECT u.UserID, u.FullName, u.RoleID, r.RoleName
                        FROM Users u
                        LEFT JOIN Roles r ON u.RoleID = r.RoleID";

            using var reader = await SqlHelper.ExecuteReaderAsync(_connectionString, sql);

            while (await reader.ReadAsync())
            {
                users.Add(new UserWithRoleViewModel
                {
                    UserID = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    RoleID = reader.GetInt32(2),
                    RoleName = reader.GetString(3),
                    AllRoles = roles
                });
            }

            return users;
        }
    }
}
