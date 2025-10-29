using System.ComponentModel.DataAnnotations;

namespace Library_backend.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Please Enter Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string password { get; set; }

        // Optional field for admin login code (not required for normal users)
        public string? code { get; set; }
    }
}
