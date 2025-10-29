using System.ComponentModel.DataAnnotations;

namespace Library_backend.ViewModels
{
    public class AdminLoginModel
    {
        [Required(ErrorMessage = "Please Enter Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string password { get; set; }

        [Required(ErrorMessage = "Please Enter Admin Code")]
        public string code { get; set; } // Required for admin login
    }
}
