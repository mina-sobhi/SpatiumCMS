using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class SpatuimDTORefgister
    {
        [Required(ErrorMessage = "FullName Is Required ")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "UserName Is Required ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required ")]
        public string Password { get; set; }

        [Compare(nameof(Password) ,ErrorMessage ="Not Matched")]
        [Required(ErrorMessage = "ConfirmPassword Is Required ")]
        public string ConfirmPassword { get; set; }
    }
}
