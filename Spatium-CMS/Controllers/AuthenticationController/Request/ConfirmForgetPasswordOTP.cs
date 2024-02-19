namespace Spatium_CMS.Controllers.AuthenticationController.Request
{
    public class ConfirmForgetPasswordOTP
    {
        public string Email { get; set; }
        public string OTP { get; set; }
    }
}