using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Request
{
    public class UpdateUserAsSuperAdminRequest
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }

        [Required(ErrorMessage = "Full Name is Required ")]
        [MaxLength(60, ErrorMessage = "full name must be less Than 60 char or equal")]
        [MinLength(3, ErrorMessage = "full name must be greater Than 3 char or equal")]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Full Name Must Be Only Char")]
        public string FullName { get; set; }

        public string Phone { get; set; }
    }
}
