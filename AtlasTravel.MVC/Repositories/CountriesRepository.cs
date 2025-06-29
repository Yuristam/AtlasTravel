using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;

namespace AtlasTravel.MVC.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly string _connectionString;

        public CountriesRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Task CreateCountryAsync(Country country)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCountryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ICollection<Country>> GetAllCountriesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Country> GetCountryByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCountryAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
