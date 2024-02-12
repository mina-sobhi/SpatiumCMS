using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "UserName Is Required ")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Is Required ")]
        public string Password { get; set; }
    }
}
