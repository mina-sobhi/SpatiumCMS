namespace Spatium_CMS.Controllers.AuthenticationController.Request
{
    public class ConfirmForgetPassword
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

    }
}
