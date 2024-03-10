using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Request
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Full Name is Required ")]
        [MaxLength(60, ErrorMessage = "full name must be less Than 60 char or equal")]
        [MinLength(3, ErrorMessage = "full name must be greater Than 3 char or equal")]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Full Name Must Be Only Char")]
        public string FullName { get; set; }

        //[Required(ErrorMessage = "Role Id is Required ")]
        //public string RoleId { get; set; }
        public IFormFile? ImageProfile { get; set; }

        //[Required(ErrorMessage = "Email is Required ")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Phone is Required ")]
        public string PhoneNumber { get; set; }
    }
}
