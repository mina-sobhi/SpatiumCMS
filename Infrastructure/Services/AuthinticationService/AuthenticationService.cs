using Domain.ApplicationUserAggregate;
using Domain.Interfaces;
using Domian.Interfaces;
using Infrastructure.Services.AuthinticationService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Utilities.Helpers;
using Utilities.Results;

namespace Infrastructure.Services.AuthinticationService
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly ISendMailService mailService;
        private readonly AuthConfig authConfig;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IUnitOfWork unitOfWork;
        public AuthenticationService(UserManager<ApplicationUser> userManager, RoleManager<UserRole> roleManager, ISendMailService mailService, AuthConfig jwtSetting, ILogger<AuthenticationService> logger, IUnitOfWork unitOfWork)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.mailService = mailService;
            this.authConfig = jwtSetting;
            _logger = logger;
            this.unitOfWork = unitOfWork;
        }

        public async Task<ApplicationUser> GetUserDetailes(string userId)
        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task ChangeUserActivation(string userId)
        {
            var user = await userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.ChangeActivation();
        }

        public async Task<SpatiumResponse> ConfirmForgetPassword(string email, string token, string newPassword)
        {
            _logger.LogInformation("Confirm Change Email started for email {email} at {time}", email, DateTime.UtcNow);
            var user = await userManager.FindByEmailAsync(email);
            var decodedToken = WebUtility.UrlDecode(token);
            if (user != null)
            {
                var result = await userManager.ResetPasswordAsync(user, decodedToken, newPassword);
                user.ClearOTP();
                await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return new SpatiumResponse()
                    {
                        Success = true,
                        Message = ResponseMessages.PasswordChangedSuccessfully
                    };
                }
                return new SpatiumResponse()
                {
                    Success = false,
                    Message = string.Join('\n', result.Errors.Select(x => x.Description).ToArray())
                };
            }
            return new SpatiumResponse()
            {
                Success = false,
                Message = ResponseMessages.InvalidEmail
            };
        }

        public async Task<SpatiumResponse> ConfirmOTP(string email, string token, string otp)
        {
            _logger.LogInformation("Confirm OTP started for email {email} at {time}", email, DateTime.UtcNow);
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.OTP != null && !user.EmailConfirmed && user.OTP.Equals(otp))
                {
                    if (DateTime.UtcNow >= user.OTPGeneratedAt.Value.AddMinutes(30))
                    {
                        user.ChangeOTP(OTPGenerator.GenerateOTP());
                        await userManager.UpdateAsync(user);
                        await mailService.SendMail(user.Email, "Verification Email", user.OTP);
                        return new SpatiumResponse()
                        {
                            Success = false,
                            Message = ResponseMessages.OTPExpired
                        };
                    }
                    var decodedToken = WebUtility.UrlDecode(token);
                    var identityResult = await userManager.ConfirmEmailAsync(user, decodedToken);
                    if (identityResult.Succeeded)
                    {
                        _logger.LogInformation("Email Confirmed forr user {user} at {time}", user, DateTime.UtcNow);
                        //To be modified using unit of work to save changes once (turn of auto save of identity)
                        user.ClearOTP();
                        await userManager.UpdateAsync(user);
                        return new SpatiumResponse
                        {
                            Success = true,
                            Message = ResponseMessages.EmailConfirmedSuccessfully
                        };
                    }
                    return new SpatiumResponse
                    {
                        Success = false,
                        Message = string.Join('\n', identityResult.Errors.Select(x => x.Description).ToArray()),
                    };
                }
            }
            _logger.LogInformation("Email Not Found {email} at {time}", email, DateTime.UtcNow);

            return new SpatiumResponse()
            {
                Success = false,
                Message = ResponseMessages.InvalidOTP
            };
        }

        public async Task<SpatiumResponse<string>> ForgetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebUtility.UrlEncode(token);
                user.ChangeOTP(OTPGenerator.GenerateOTP());
                await mailService.SendMail(user.Email, "Spatium CMS Verification Email!", $"Your OTP is: {user.OTP}.");
                await userManager.UpdateAsync(user);
                return new SpatiumResponse<string>()
                {
                    Success = true,
                    Data = encodedToken,
                    Message = ResponseMessages.ForgetPasswordEmailSent
                };
            }
            return new SpatiumResponse<string>()
            {
                Success = false,
                Message = ResponseMessages.ForgetPasswordEmailSent
            };
        }

        public async Task<SpatiumResponse<LoggedInUser>> Login(string email, string password)
        {
            _logger.LogInformation("Login started for user {email} at {time}", email, DateTime.UtcNow);
            var user = await userManager.FindByEmailAsync(email);
            if (user != null && await userManager.CheckPasswordAsync(user, password))
            {
                if (!user.EmailConfirmed)
                {
                    var otpResult = await ResendConfirmationEmailOTP(user.Email);
                    return new SpatiumResponse<LoggedInUser>()
                    {
                        Success = false,
                        Message = ResponseMessages.EmailNotConfirmed,
                        Data = new LoggedInUser()
                        {
                            Token = otpResult.Data
                        },
                    };
                }
                var tokenParams = await GenerateToken(user);
                var loggedInUser = new LoggedInUser()
                {
                    Email = email,
                    ExpireDate = tokenParams.ExpireDate,
                    FullName = user.FullName,
                    Token = tokenParams.Token,
                    EmailConfirmed = true
                };
                return new SpatiumResponse<LoggedInUser>()
                {
                    Success = true,
                    Data = loggedInUser,
                };
            }
            return new SpatiumResponse<LoggedInUser>()
            {
                Success = false,
                Message = ResponseMessages.InvalidEmailOrPassword,
            };
        }

        public async Task<SpatiumResponse<string>> Register(ApplicationUser newUser, string password)
        {
            _logger.LogInformation("Registeration Started. user: {user}", newUser);
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user != null)
            {
                _logger.LogInformation("User is already registered. user: {user}", newUser);
                return new SpatiumResponse<string>()
                {
                    Success = false,
                    Message = ResponseMessages.EmailIsAlreadyExist
                };
            }
            newUser.ChangeOTP(OTPGenerator.GenerateOTP());
            var createUserResult = await userManager.CreateAsync(newUser, password);
            if (createUserResult.Succeeded)
            {
                var token = await userManager.GenerateEmailConfirmationTokenAsync(newUser);
                var encodedToken = WebUtility.UrlEncode(token);
                _logger.LogInformation("User Created Successfully. user {user} at {date}", newUser, DateTime.UtcNow);
                await mailService.SendMail(newUser.Email, "Spatium CMS Verification Email!", $"Your OTP is: {newUser.OTP}.");
                return new SpatiumResponse<string>()
                {
                    Success = true,
                    Message = ResponseMessages.UserCreatedSuccessfully,
                    Data = encodedToken
                };
            }
            return new SpatiumResponse<string>()
            {
                Success = false,
                Message = string.Join(System.Environment.NewLine, createUserResult.Errors.Select(x => x.Description).ToArray())
            };
        }

        public async Task<SpatiumResponse<string>> ResendConfirmationEmailOTP(string email)
        {
            _logger.LogInformation("Resend Confirmation Email OTP Started for email: {email}", email);
            var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    return new SpatiumResponse<string>()
                    {
                        Message = ResponseMessages.EmailIsAlreadyConfirmed,
                        Success = false,
                    };
                }
                var newOtp = OTPGenerator.GenerateOTP();
                var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                var encodedToken = WebUtility.UrlEncode(token);
                user.ChangeOTP(newOtp);
                await userManager.UpdateAsync(user);
                await mailService.SendMail(user.Email, "Spatium CMS Verification Email!", $"Your OTP is: {user.OTP}.");
                _logger.LogInformation("ResendConfirmationEmailOTP: Confirmation Email OTP Started for user: {user}", user);
                return new SpatiumResponse<string>()
                {
                    Message = ResponseMessages.VerificationEmailSent,
                    Success = true,
                    Data = encodedToken
                };
            }
            return new SpatiumResponse<string>()
            {
                Message = ResponseMessages.VerificationEmailSent,
                Success = true,
            };
        }

        private async Task<TokenParameters> GenerateToken(ApplicationUser user)
        {
            _logger.LogDebug("Generating Token for user {email} at {time}", user.Email, DateTime.UtcNow);
            var permissions = await unitOfWork.RoleRepository.GetRolePermissionIds(user.RoleId);
            var claims = new List<Claim>()
            {
                new (ClaimTypes.NameIdentifier,user.Id),
                new (ClaimTypes.Email,user.Email),
                new ("RoleId",user.RoleId)
            };
            foreach (var item in permissions)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.SecretKey));
            var token = new JwtSecurityToken(
                    issuer: authConfig.ValidIssuer,
                    audience: authConfig.ValidAudience,
                    expires: DateTime.UtcNow.AddDays(authConfig.TokenExpireInDays),
                    claims: claims,
                    signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
                );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            var tokenExpireDate = token.ValidTo;
            _logger.LogInformation("Token Generated for user: {email} at {time}", user.Email, DateTime.UtcNow);
            return new TokenParameters(tokenStr, tokenExpireDate);
        }
    }
}
