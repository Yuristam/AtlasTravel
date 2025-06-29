using Microsoft.AspNetCore.Mvc;

namespace AtlasTravel.MVC.Controllers
{
    [Route("countries")]
    public class CountriesController : Controller
    {
        public CountriesController() { }

        public IActionResult Index()
        {
            return View();
        }

    }
}
