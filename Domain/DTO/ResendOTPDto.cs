using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class ResendOTPDto
    {
        [Required(ErrorMessage = "UserName Is Required ")]
        public string Email { get; set; }
    }
}
