using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using AtlasTravel.MVC.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AtlasTravel.MVC.Controllers
{
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IUsersRepository _usersRepository;

        public AuthController(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        [HttpGet("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(registerViewModel);
            }
            try
            {
                var existingUser = await _usersRepository.GetUserByEmailAsync(registerViewModel.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email уже используется.");
                    return View(registerViewModel);
                }

                var user = new User
                {
                    FullName = registerViewModel.FullName,
                    Email = registerViewModel.Email,
                    Password = registerViewModel.Password,
                    Budget = 0
                };

                await _usersRepository.CreateUserAsync(user);
                return RedirectToAction("SignIn");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при регистрации: {ex.Message}");
                return View(registerViewModel);
            }
        }

        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(loginViewModel);
            }

            var existingUser = await _usersRepository.GetUserByEmailAsync(loginViewModel.Email);
            if (existingUser == null || existingUser.Password != loginViewModel.Password)
            {
                ModelState.AddModelError("", "Неверный email или пароль.");
                return View(loginViewModel);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, existingUser.UserID.ToString()),
                new Claim(ClaimTypes.Name, existingUser.FullName),
                new Claim(ClaimTypes.Email, existingUser.Email),
                new Claim(ClaimTypes.Role, existingUser.RoleName)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Home");
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}