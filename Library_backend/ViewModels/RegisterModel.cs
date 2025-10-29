using System.ComponentModel.DataAnnotations;

namespace Library_backend.ViewModels
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is Required.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email Address is Required.")]
        [EmailAddress(ErrorMessage = "Please Provide Valid Email Address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password & Confirm Password is not matching")]
        [Required(ErrorMessage = "This Confirm Password is Required.")]
        public string ConfirmPassword { get; set; }
    }
}
