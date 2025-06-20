using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace AtlasTravel.MVC.Controllers
{
    [Route("admin")]
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public AdminController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet("")]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            var users = await _usersRepository.GetAllUsersAsync();

            return View(users);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> UserDetails(int id)
        {
            var user = await _usersRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpGet("create-user")]
        public IActionResult CreateUser()
        {
            return View();
        }

        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                await _usersRepository.CreateUserAsync(user);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при сохранении данных. {ex.Message}");
                return View(user);
            }
        }

        [HttpGet("edit-user/{id}")]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _usersRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("edit-user/{id}")]
        public async Task<IActionResult> EditUser(int id, User user)
        {
            if (id != user.UserID)
            {
                ModelState.AddModelError("", "Неверный идентификатор.");
                return View(user);
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            try
            {
                await _usersRepository.UpdateUserAsync(user);
                return RedirectToAction("Index");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при изменении данных. {ex.Message}");
                return View(user);
            }
        }

        [HttpGet("delete-user/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _usersRepository.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("delete-user/{id}"), ActionName("Delete")]
        public async Task<IActionResult> DeleteUserConfirmed(int id)
        {
            try
            {
                await _usersRepository.DeleteUserAsync(id);
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
