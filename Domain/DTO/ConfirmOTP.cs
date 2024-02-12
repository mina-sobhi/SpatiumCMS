using System.ComponentModel.DataAnnotations;

namespace DemoIdentity.DTO
{
    public class ConfirmOTP
    {
        [Required(ErrorMessage = "UserName Is Required ")]
        public string Email { get; set; }
        [Required(ErrorMessage = "OTP Is Required ")]
        public string OTP { get; set; }
    }
}
