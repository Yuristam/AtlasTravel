using AtlasTravel.MVC.Models;

namespace AtlasTravel.MVC.Interfaces
{
    public interface ICountriesRepository
    {
        Task<ICollection<Country>> GetAllCountriesAsync();
        Task<Country> GetCountryByIdAsync(int id);

        Task CreateCountryAsync(Country country);
        Task UpdateCountryAsync(Country country);
        Task DeleteCountryAsync(int id);
    }
}
