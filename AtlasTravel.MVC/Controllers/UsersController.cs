using AtlasTravel.MVC.Dtos;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace AtlasTravel.MVC.Controllers
{
    [Authorize]
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

            if (user == null)
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

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);
            return View(user);
        }

        [HttpGet("editprofile")]
        public async Task<IActionResult> EditProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("editprofile")]
        public async Task<IActionResult> EditProfile(EditUserDto userDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (userId != userDto.UserID)
            {
                ModelState.AddModelError("", "Неверный идентификатор.");
                return View(userDto);
            }

            if (!ModelState.IsValid)
            {
                return View(userDto);
            }

            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(userId);
                existingUser.FullName = userDto.FullName;
                existingUser.Budget = userDto.Budget;

                await _userRepository.UpdateUserAsync(existingUser);
                return RedirectToAction("Profile");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при изменении данных. {ex.Message}");
                return View(userDto);
            }
        }

        [HttpGet("changepassword")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();
            
            if (user.Password != dto.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Текущий пароль неверен.");
                return View();
            }

            await _userRepository.ChangePassword(userId, dto.NewPassword);

            TempData["PasswordChanged"] = "Пароль успешно обновлён.";
            return RedirectToAction("EditProfile");
        }

        [HttpGet("delete")]
        public async Task<IActionResult> Delete()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("delete"), ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);

            try
            {
                await _userRepository.DeleteUserAsync(userId);
                await HttpContext.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при удалении данных. {ex.Message}");
                return View();
            }
        }      
    }
}
