using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class ChangePasswordDTO
    {
        [Required(ErrorMessage = "Email Is Required ")]
        public string Email { get; set; }

        public string Code { get; set; }
        [Required(ErrorMessage = " New Password Is Required ")]
        public string NewPassword { get; set; }

        [Compare(nameof(NewPassword), ErrorMessage = "Not Matched")]
        [Required(ErrorMessage = "new ConfirmPassword Is Required ")]
        public string NewConfirmPassword { get; set; }
    }
}
