using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using AtlasTravel.MVC.ViewModels;
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
        private readonly IAdminRepository _adminRepository;

        public AdminController(IUsersRepository usersRepository, IAdminRepository adminRepository)
        {
            _usersRepository = usersRepository;
            _adminRepository = adminRepository;
        }

        [HttpGet("")]
        public async Task<IActionResult> Dashboard()
        {
            var model = new AdminDashboardViewModel
            {
                TotalUsers = await _adminRepository.CountUsersAsync(),
            };

            return View(model);
        }

        [HttpGet("admin/users")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _adminRepository.GetAllUsersWithRolesAsync();

            return View("ManageUsers", users);
        }

        [HttpPost("admin/assign-role")]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            await _adminRepository.AssignRoleAsync(userId, roleId);
            return RedirectToAction("ManageUsers");
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
        public async Task<IActionResult> CreateUser(CreateUserViewModel createUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserViewModel);
            }

            try
            {
                var existingUser = await _usersRepository.GetUserByEmailAsync(createUserViewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email уже используется.");
                    return View(createUserViewModel);
                }

                var user = new User
                {
                    FullName = createUserViewModel.FullName,
                    Email = createUserViewModel.Email,
                    Password = createUserViewModel.Password,
                    Budget = createUserViewModel.Budget
                };

                await _usersRepository.CreateUserAsync(user);
                return RedirectToAction("Users");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при сохранении данных. {ex.Message}");
                return View(createUserViewModel);
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

            var model = new EditUserViewModel
            {
                UserID = user.UserID,
                FullName = user.FullName,
                Budget = user.Budget,
                OldPassword = user.Password
            };

            return View(model);
        }

        [HttpPost("edit-user/{id}")]
        public async Task<IActionResult> EditUser(int id, EditUserViewModel userEditViewModel)
        {
            if (id != userEditViewModel.UserID)
            {
                ModelState.AddModelError("", "Неверный идентификатор.");
                return View(userEditViewModel);
            }

            if (!ModelState.IsValid)
            {
                return View(userEditViewModel);
            }

            try
            {
                var existingUser = await _usersRepository.GetUserByIdAsync(id);
                existingUser.FullName = userEditViewModel.FullName;
                existingUser.Budget = userEditViewModel.Budget;

                if (userEditViewModel.NewPassword != null && userEditViewModel.ConfirmPassword != null)
                {
                    existingUser.Password = userEditViewModel.NewPassword;
                }

                await _adminRepository.UpdateUserByAdminAsync(existingUser);
                return RedirectToAction("Users");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при изменении данных. {ex.Message}");
                return View(userEditViewModel);
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
                return RedirectToAction("Users");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при удалении данных. {ex.Message}");
                return View();
            }
        }
    }
}
