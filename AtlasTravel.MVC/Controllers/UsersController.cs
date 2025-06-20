using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.ViewModels;
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

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);
            return View(user);
        }

        [HttpGet("edit-profile")]
        public async Task<IActionResult> EditProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            var model = new EditUserProfileViewModel
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Budget = user.Budget
            };

            return View(model);
        }

        [HttpPost("edit-profile")]
        public async Task<IActionResult> EditProfile(EditUserProfileViewModel userViewModel)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (userId != userViewModel.UserID)
            {
                ModelState.AddModelError("", "Неверный идентификатор.");
                return View(userViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(userViewModel);
            }

            try
            {
                var existingUser = await _userRepository.GetUserByIdAsync(userId);
                existingUser.FullName = userViewModel.FullName;
                existingUser.Budget = userViewModel.Budget;

                await _userRepository.UpdateUserAsync(existingUser);
                return RedirectToAction("Profile");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при изменении данных. {ex.Message}");
                return View(userViewModel);
            }
        }

        [HttpGet("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel ChangePasswordVM)
        {
            if (!ModelState.IsValid)
            {
                return View(ChangePasswordVM);
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null) return NotFound();
            
            if (user.Password != ChangePasswordVM.CurrentPassword)
            {
                ModelState.AddModelError("CurrentPassword", "Текущий пароль неверен.");
                return View();
            }

            await _userRepository.ChangePassword(userId, ChangePasswordVM.NewPassword);

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
