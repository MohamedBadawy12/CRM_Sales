using System.ComponentModel.DataAnnotations;

namespace CRM_Sales_Application.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "ُEmail is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }

    public class RegisterDto
    {
        public string? FullName { get; set; }

        [Required(ErrorMessage = "ُEmail is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage = "Password must be lower than 8 numbers")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Two Passwords are not matchs")]
        public string ConfirmPassword { get; set; }
    }
}
