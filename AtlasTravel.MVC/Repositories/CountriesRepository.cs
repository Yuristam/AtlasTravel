using AtlasTravel.MVC.Helpers;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly string _connectionString;

        public CountriesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task CreateCountryAsync(Country country)
        {
            var sql = "INSERT INTO Coutries VALUES (@CountryName)";

            var parameters = new[] { new SqlParameter("@CountryName", country.CountryName) };

            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public async Task DeleteCountryAsync(int id)
        {
            var sql = "DELETE FROM Countries WHERE CountryID = @CountryID";
            var parameters = new[] { new SqlParameter("@CountryID", id) };
            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }

        public async Task<ICollection<Country>> GetAllCountriesAsync()
        {
            var countries = new List<Country>();

            var sql = "SELECT * FROM Countries";

            using var reader = await SqlHelper.ExecuteReaderAsync(_connectionString, sql);

            while (await reader.ReadAsync())
            {
                countries.Add(new Country
                {
                    CountryID = reader.GetByte(0),
                    CountryName = reader.GetString(1),
                });
            }

            return countries;
        }

        public async Task<Country> GetCountryByIdAsync(int id)
        {
            var sql = "SELECT * FROM Countries WHERE CountryID = @ID";
            var parameters = new[] { new SqlParameter("@ID", id) };

            using var reader = await SqlHelper.ExecuteReaderAsync(_connectionString, sql, parameters);

            if (await reader.ReadAsync())
            {
                return new Country
                {
                    CountryID = reader.GetByte(0),
                    CountryName = reader.GetString(1),
                };
            }

            return null;
        }

        public async Task UpdateCountryAsync(Country country)
        {
            var sql = @"UPDATE Countries
                        SET CountryName = @CountryName
                        WHERE CountryID = @CountryID";

            var parameters = new[] {
                new SqlParameter("@CountryName", country.CountryName),
                new SqlParameter("@CountryID", country.CountryID)
            };

            await SqlHelper.ExecuteNonQueryAsync(_connectionString, sql, parameters);
        }
    }
}
