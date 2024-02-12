using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.AuthenticationController.Request
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Full Name is Required ")]
        [MaxLength(60,ErrorMessage = "full name must be less Than 60 char or equal")]
        [MinLength(3, ErrorMessage = "full name must be greater Than 3 char or equal")]
        [RegularExpression(@"^[a-zA-Z_ ]+$",ErrorMessage = "Full Name Must Be Only Char")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Job Title is Required")]
        [MaxLength(60, ErrorMessage = "Job Title must be less Than 60 char or equal")]
        [MinLength(3, ErrorMessage = "Job Title must be greater Than 3 char or equal")]
        [RegularExpression(@"^[a-zA-Z_ ]+$", ErrorMessage = "Job Title Must Be Only Char")]
        public string JobTitle { get; set; }

        [Required(ErrorMessage = "Email is Required ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required ")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Password doesn't Match")]
        [Required(ErrorMessage = "ConfirmPassword is Required ")]
        public string ConfirmPassword { get; set; }
    }
}
