using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class RegisterDto
    {
        [Required(ErrorMessage ="FullName Is Required ")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "UserName Is Required ")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Password Is Required ")]
        public string Password { get; set; }
    }
}
