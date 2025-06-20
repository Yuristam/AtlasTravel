using System.ComponentModel.DataAnnotations;

namespace AtlasTravel.MVC.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        public decimal? Budget { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, MinLength(6)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string ConfirmPassword { get; set; }
    }
}
