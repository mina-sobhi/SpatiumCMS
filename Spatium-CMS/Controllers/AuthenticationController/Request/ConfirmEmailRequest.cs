namespace Spatium_CMS.Controllers.AuthenticationController.Request
{
    public class ConfirmEmailRequest
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        public string Token { get; set; }
    }
}
