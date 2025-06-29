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
        private static readonly string[] SYSTEM_ROLES = { "ADMIN", "USER" };
        private readonly IUsersRepository _usersRepository;
        private readonly IAdminRepository _adminRepository;
        private readonly IRolesRepository _rolesRepository;
        private readonly IPermissionsRepository _permissionsRepository;

        public AdminController(IUsersRepository usersRepository, IAdminRepository adminRepository, 
            IRolesRepository rolesRepository, IPermissionsRepository permissionsRepository)
        {
            _usersRepository = usersRepository;
            _adminRepository = adminRepository;
            _rolesRepository = rolesRepository;
            _permissionsRepository = permissionsRepository;
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

        [HttpGet("users")]
        public async Task<IActionResult> ManageUsers()
        {
            var users = await _usersRepository.GetAllUsersAsync();

            return View("ManageUsers", users);
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
                return RedirectToAction("ManageUsers");
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
                return RedirectToAction("ManageUsers");
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
                return RedirectToAction("ManageUsers");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при удалении данных. {ex.Message}");
                return View();
            }
        }

        [HttpGet("user-role-management")]
        public async Task<IActionResult> UserRoleManagement()
        {
            var users = await _adminRepository.GetAllUsersWithRolesAsync();
            return View("UserRoleManagement", users);
        }

        [HttpGet("roles")]
        public async Task<IActionResult> ManageRoles()
        {
            var roles = await _rolesRepository.GetAllRolesAsync();
            return View("ManageRoles", roles);
        }

        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(int userId, int roleId)
        {
            await _rolesRepository.AssignRoleToUserAsync(userId, roleId);
            return RedirectToAction("UserRoleManagement");
        }

        [HttpPost("create-role")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                ModelState.AddModelError("RoleName", "Имя роли не может быть пустым.");
                return View("UserRoleManagement");
            }
            try
            {
                await _rolesRepository.CreateRoleAsync(roleName);
                return RedirectToAction("UserRoleManagement");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при создании роли. {ex.Message}");
                return View("UserRoleManagement");
            }
        }

        [HttpPost("delete-role")]
        public async Task<IActionResult> DeleteRole(int roleId)
        {
            try
            {
                var role = await _rolesRepository.GetRoleByIdAsync(roleId);
                var users = await _rolesRepository.GetUsersByRoleAsync(roleId);

                if (role == null)
                    return NotFound();

                if (SYSTEM_ROLES.Contains(role.RoleName.ToUpper()))
                    return BadRequest($"Нельзя удалить системную роль {role.RoleName}.");

                if (users.Any(u => u.RoleID == roleId))
                {
                    return BadRequest("Нельзя удалить роль, пока есть пользователи с этой ролью.");
                }

                await _rolesRepository.DeleteRoleAsync(roleId);
                return RedirectToAction("UserRoleManagement");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при удалении роли. {ex.Message}");
                return View("UserRoleManagement");
            }
        }

        [HttpGet("roles/{roleId}/permissions")]
        public async Task<IActionResult> ManagePermissions(int roleId) {

            var viewModel = await _permissionsRepository.GetRolePermissionsAsync(roleId);

            if (viewModel == null)
                return NotFound();

            return View("ManagePermissions", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePermissions(int roleId, int[] selectedPermissionIds)
        {
            await _permissionsRepository.ClearPermissionsAsync(roleId);
            foreach (var permId in selectedPermissionIds)
            {
                await _permissionsRepository.AssignPermissionToRoleAsync(roleId, permId);
            }

            return RedirectToAction("ManageRoles");
        }
    }
}
