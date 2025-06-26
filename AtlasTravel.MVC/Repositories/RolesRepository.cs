using AtlasTravel.MVC.Helpers;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using AtlasTravel.MVC.ViewModels;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Repositories
{
    public class RolesRepository : IRolesRepository
    {
        private readonly string _connectionString;

        public RolesRepository(string connectionString)
        {
            _connectionString = connectionString;    
        }

        public Task AssignRoleToUserAsync(int userId, int roleId)
        {
            throw new NotImplementedException();
        }

        public async Task CreateRoleAsync(string roleName)
        {
            var sql = "INSERT INTO Roles (RoleName) VALUES (@RoleName)";
            var parameters = new[] { new SqlParameter("@RoleName", roleName) };
            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public Task DeleteRoleAsync(int roleId)
        {
            var sql = "DELETE FROM Roles WHERE RoleID = @RoleID";
            var parameters = new[] { new SqlParameter("@RoleID", roleId) };
            return SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public Task<List<Role>> GetAllRolesAsync()
        {
            var roles = new List<Role>();
            var sql = "SELECT * FROM Roles";
            using var reader = SqlHelper.ExecuteReaderAsync(_connectionString, sql).Result;
            while (reader.Read())
            {
                roles.Add(new Role
                {
                    RoleID = (int)reader["RoleID"],
                    RoleName = (string)reader["RoleName"]
                });
            }
            return Task.FromResult(roles);

        }

        public Task<Role> GetRoleByIdAsync(int roleId)
        {
            var sql = "SELECT * FROM Roles WHERE RoleID = @RoleID";
            var parameters = new[] { new SqlParameter("@RoleID", roleId) };
            using var reader = SqlHelper.ExecuteReaderAsync(_connectionString, sql, parameters).Result;
            if (reader.Read())
            {
                return Task.FromResult(new Role
                {
                    RoleID = (int)reader["RoleID"],
                    RoleName = (string)reader["RoleName"]
                });
            }
            return Task.FromResult<Role>(null);

        }

        public Task<List<UserWithRoleViewModel>> GetUsersByRoleAsync(int roleId)
        {
            var users = new List<UserWithRoleViewModel>();
            var sql = @"SELECT u.UserID, u.FullName, u.RoleID, r.RoleName
                        FROM Users u
                        JOIN Roles r ON u.RoleID = r.RoleID
                        WHERE r.RoleID = @RoleID";
            var parameters = new[] { new SqlParameter("@RoleID", roleId) };
            using var reader = SqlHelper.ExecuteReaderAsync(_connectionString, sql, parameters).Result;
            while (reader.Read())
            {
                users.Add(new UserWithRoleViewModel
                {
                    UserID = reader.GetInt32(0),
                    FullName = reader.GetString(1),
                    RoleID = reader.GetInt32(2),
                    RoleName = reader.GetString(3)
                });
            }
            return Task.FromResult(users);
        }

        public Task RemoveRoleFromUserAsync(int userId, int roleId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateRoleAsync(Role role)
        {
            throw new NotImplementedException();
        }
    }
}
