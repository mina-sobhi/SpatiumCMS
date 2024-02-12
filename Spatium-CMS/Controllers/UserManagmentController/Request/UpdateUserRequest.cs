using System.ComponentModel.DataAnnotations;

namespace Spatium_CMS.Controllers.UserManagmentController.Request
{
    public class UpdateUserRequest
    {
        [Required(ErrorMessage = "Full Name is Required ")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Role Id is Required ")]
        public string RoleId { get; set; }

        [Required(ErrorMessage = " Iamge is Required ")]
        public string ProfileImagePath { get; set; }

        //[Required(ErrorMessage = "Email is Required ")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "Phone is Required ")]
        public string PhoneNumber { get; set; }
    }
}
