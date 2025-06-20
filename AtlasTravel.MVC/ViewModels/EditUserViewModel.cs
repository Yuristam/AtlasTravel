using System.ComponentModel.DataAnnotations;

namespace AtlasTravel.MVC.ViewModels
{
    public class EditUserViewModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Полное имя обязательно")]
        [StringLength(100, ErrorMessage = "Полное имя не может превышать 100 символов")]
        public string FullName { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Бюджет должен быть неотрицательным числом")]
        public decimal? Budget { get; set; }

        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }
    }
}
