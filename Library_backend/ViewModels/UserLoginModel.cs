using System.ComponentModel.DataAnnotations;

namespace Library_backend.ViewModels
{
    public class UserLoginModel
    {
        [Required(ErrorMessage = "Please Enter Username")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please Enter Password")]
        public string password { get; set; }
    }
}
