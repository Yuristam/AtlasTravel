using Microsoft.Data.SqlClient;
using System.Data;

namespace AtlasTravel.MVC.Helpers
{
    public static class SqlHelper
    {
        public static async Task<int> ExecuteNonQueryAsync(
            string connectionString,
            string sql,
            SqlParameter[]? parameters = null)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            if (parameters != null) 
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            return await command.ExecuteNonQueryAsync();
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(
            string connectionString,
            string sql,
            SqlParameter[]? parameters = null)
        {
            var connection = new SqlConnection(connectionString);
            var command = new SqlCommand(sql, connection);
            if(parameters != null)
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();

            return await command.ExecuteReaderAsync(CommandBehavior.CloseConnection);
        }

        public static async Task<object?> ExecuteScalarAsync(
            string connectionString,
            string sql,
            SqlParameter[]? parameters = null)
        {
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);
            if (parameters != null)
                command.Parameters.AddRange(parameters);

            await connection.OpenAsync();
            return await command.ExecuteScalarAsync();
        }
    }
}
