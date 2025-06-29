using AtlasTravel.MVC.Helpers;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using AtlasTravel.MVC.ViewModels;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Repositories
{
    public class PermissionsRepository : IPermissionsRepository
    {
        private readonly string _connectionString;

        public PermissionsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task ClearPermissionsAsync(int roleId)
        {
            var sql = "DELETE FROM RolePermissions WHERE RoleID = @RoleID";

            var parameters = new[] { new SqlParameter("@RoleID", roleId) };

            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public async Task AssignPermissionToRoleAsync(int roleId, int permissionId)
        {
            var sql = "INSERT INTO RolePermissions (RoleID, PermissionID) VALUES (@RoleID, @PermissionID)";
            var parameters = new[]
            {
                new SqlParameter("@RoleID", roleId),
                new SqlParameter("@PermissionID", permissionId)
            };
            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public async Task<ManageRolePermissionsViewModel> GetRolePermissionsAsync(int roleId)
        {
            var model = new ManageRolePermissionsViewModel();

            // Получить роль
            var roleSql = "SELECT RoleID, RoleName FROM Roles WHERE RoleID = @RoleID";
            using var roleReader = await SqlHelper.ExecuteReaderAsync(
                _connectionString, roleSql, new[] { new SqlParameter("@RoleID", roleId) });

            if (await roleReader.ReadAsync())
            {
                model.RoleID = roleReader.GetInt32(0);
                model.RoleName = roleReader.GetString(1);
            }

            // Получить все разрешения
            var permissionsSql = "SELECT PermissionID, PermissionName FROM Permissions";
            using var permsReader = await SqlHelper.ExecuteReaderAsync(_connectionString, permissionsSql);
            while (await permsReader.ReadAsync())
            {
                model.AllPermissions.Add(new Permission
                {
                    PermissionID = permsReader.GetInt32(0),
                    PermissionName = permsReader.GetString(1)
                });
            }

            // Получить назначенные разрешения для роли
            var assignedSql = "SELECT PermissionID FROM RolePermissions WHERE RoleID = @RoleID";
            using var assignedReader = await SqlHelper.ExecuteReaderAsync(
                _connectionString, assignedSql, new[] { new SqlParameter("@RoleID", roleId) });

            while (await assignedReader.ReadAsync())
            {
                model.AssignedPermissions.Add(assignedReader.GetInt32(0));
            }

            return model;
        }
    }
}
