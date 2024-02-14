namespace Spatium_CMS.Controllers.AuthenticationController.Response
{
    public class ConfirmForgetPasswordOtpResponse
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
