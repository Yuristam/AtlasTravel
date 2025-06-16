using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUsersRepository _userRepository;

        public UsersController(IUsersRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsersAsync();

            return View(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user != null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                await _userRepository.CreateUserAsync(user);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при сохранении данных. {ex.Message}");
                return View(user);
            }
        }

        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost("edit/{id}")]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                await _userRepository.UpdateUserAsync(user);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при изменении данных. {ex.Message}");
                return View(user);
            }
        }

        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            
            if(user == null)
            {
                return NotFound();
            }

            return View();
        }

        [HttpPost("delete/{id}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                await _userRepository.DeleteUserAsync(id);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при удалении данных. {ex.Message}");
                return View();
            }
        }
    }
}
