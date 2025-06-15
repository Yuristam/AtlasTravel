using AtlasTravel.MVC.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AtlasTravel.MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersRepository _userRepository;

        public UsersController(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return View(users);
        }
    }
}
