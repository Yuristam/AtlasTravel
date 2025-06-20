using System.ComponentModel.DataAnnotations;

namespace AtlasTravel.MVC.Models
{
    public class User
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Full Name is required.")]
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public decimal? Budget { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
    }
}
