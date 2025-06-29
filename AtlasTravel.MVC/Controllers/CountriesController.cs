using AtlasTravel.MVC.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AtlasTravel.MVC.Controllers
{
    [Route("countries")]
    public class CountriesController : Controller
    {
        private readonly ICountriesRepository _countriesRepository;

        public CountriesController(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var countries = await _countriesRepository.GetAllCountriesAsync();

            return View(countries);
        }

    }
}
