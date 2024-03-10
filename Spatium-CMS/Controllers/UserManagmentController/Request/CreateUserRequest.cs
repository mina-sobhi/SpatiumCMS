using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Request
{
    public class CreateUserRequest
    {

        [Required(ErrorMessage = "Full Name is Required ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Role Id is Required ")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Email is Required ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required ")]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Password doesn't Match")]
        [Required(ErrorMessage = "ConfirmPassword is Required ")]
        public string ConfirmPassword { get; set; }
        public IFormFile? ImageProfile { get; set; }
    }
}
