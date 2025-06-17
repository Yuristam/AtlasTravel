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
        public async Task<IActionResult> SignUp(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }
            try
            {
                var existingUser = await _usersRepository.GetUserByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("Email", "Email уже используется.");
                    return View(user);
                }

                await _usersRepository.CreateUserAsync(user);
                return RedirectToAction("SignIn");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Произошла ошибка при регистрации: {ex.Message}");
                return View(user);
            }
        }

        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser = await _usersRepository.GetUserByEmailAsync(user.Email);
            if (existingUser == null || existingUser.Password != user.Password)
            {
                ModelState.AddModelError("", "Неверный email или пароль.");
                return View(user);
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