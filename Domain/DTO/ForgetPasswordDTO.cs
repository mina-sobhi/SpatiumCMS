using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class ForgetPasswordDTO
    {
        [Required(ErrorMessage = "UserName Is Required ")]
        public string Email { get; set; }
    }
}
