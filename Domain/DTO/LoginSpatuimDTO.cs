using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class LoginSpatuimDTO
    {
        [Required(ErrorMessage = "Email Is Required ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password Is Required ")]
        public string Password { get; set; }
    }
}
