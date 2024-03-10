using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Request
{
    public class CreateUserRequest
    {
        [Required(ErrorMessage = "Full Name is Required ")]
        [MaxLength(60, ErrorMessage = "full name must be less Than 60 char or equal")]
        [MinLength(3, ErrorMessage = "full name must be greater Than 3 char or equal")]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Full Name Must Be Only Char")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Role Id is Required ")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Email is Required ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required ")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$", ErrorMessage = "-At least 8 characters long\r\n- Contains at least one uppercase letter (A-Z)\r\n- Contains at least one lowercase letter (a-z)\r\n- Contains at least one digit (0-9)\r\n- Contains at least one special character")]

        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Password doesn't Match")]
        [Required(ErrorMessage = "ConfirmPassword is Required ")]
        public string ConfirmPassword { get; set; }
        public IFormFile? ImageProfile { get; set; }
    }
}
