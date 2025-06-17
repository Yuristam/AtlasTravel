using AtlasTravel.MVC.Dtos;
using AtlasTravel.MVC.Interfaces;
using AtlasTravel.MVC.Models;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> SignUp(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View(registerDto);
            }
            try
            {
                var existingUser = await _usersRepository.GetUserByEmailAsync(registerDto.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email уже используется.");
                    return View(registerDto);
                }

                var user = new User
                {
                    FullName = registerDto.FullName,
                    Email = registerDto.Email,
                    Password = registerDto.Password,
                    Budget = 0
                };

                await _usersRepository.CreateUserAsync(user);
                return RedirectToAction("SignIn");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при регистрации: {ex.Message}");
                return View(registerDto);
            }
        }

        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return View(loginDto);
            }

            var existingUser = await _usersRepository.GetUserByEmailAsync(loginDto.Email);
            if (existingUser == null || existingUser.Password != loginDto.Password)
            {
                ModelState.AddModelError("", "Неверный email или пароль.");
                return View(loginDto);
            }

            HttpContext.Session.SetInt32("UserID", existingUser.UserID);
            return RedirectToAction("Index", "Home");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("SignIn");
        }
    }
}