using Domain.ApplicationUserAggregate;
using Infrastructure.Services.AuthinticationService.Models;
using Utilities.Results;

namespace Domain.Interfaces
{
    public interface IAuthenticationService
    {
        public Task<SpatiumResponse<LoggedInUser>> Login (string email , string password);
        public Task<SpatiumResponse<string>> Register(ApplicationUser newUser, string password);
        public Task<SpatiumResponse> ConfirmOTP(string email,string token,string otp);
        public Task<SpatiumResponse<string>> ResendConfirmationEmailOTP(string email);
        public Task<SpatiumResponse<string>> ForgetPassword(string email);
        public Task<SpatiumResponse> ConfirmForgetPassword(string email,string token , string newPassword);
        public Task<ApplicationUser> GetUserDetailes(string userId);

        public Task ChangeUserActivation(string UserId);
    }
}
