using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.Interfaces;
using Domian.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spatium_CMS.Controllers.AuthenticationController.Converter;
using Spatium_CMS.Controllers.AuthenticationController.Request;
using Spatium_CMS.Controllers.AuthenticationController.Response;
using Spatium_CMS.Filters;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Results;

namespace Spatium_CMS.Controllers.AuthenticationController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : CmsControllerBase
    {
        private readonly IAuthenticationService authenticationService;
        private readonly RoleManager<UserRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public AuthenticationController(ILogger<AuthenticationController> logger, IAuthenticationService authenticationService, IMapper mapper, RoleManager<UserRole> roleManager, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, mapper, logger, userManager)
        {
            this.authenticationService = authenticationService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        [HttpGet]
        [Route("ChangeUserActivation/{userId}")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.UpdateUser)]
        public Task<IActionResult> ChangeUserActivation(string userId, bool activeStatus = true)
        {
            return TryCatchLogAsync(async () =>
            {
                var parentUserId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserByIdInBlogIgnoreFilterAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                if (user.IsAccountActive == activeStatus)
                    throw new SpatiumException(ResponseMessages.CannotChangeStatus);
                user.ChangeActivation(activeStatus);
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = ResponseMessages.UserStatusChanged,
                    Success = true,
                };
                return Ok(response);
            });
        }

        [HttpGet]
        [Route("GetUserDetails")]
        [Authorize]
        public Task<IActionResult> GetUserDetails()
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var currentuser = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);

                var userdetailes = await authenticationService.GetUserDetailes(currentuser.Id);
                var detailesResult = mapper.Map<ViewUserProfileResult>(userdetailes);
                return Ok(detailesResult);
            });
        }

        [HttpPost]
        [Route("Register")]
        public Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return TryCatchLogAsync(async () =>
            {
                var converter = new AuthenticationConverter(mapper);
                var role = await roleManager.Roles.FirstOrDefaultAsync(x => x.Name == "Super Admin");
                var userInput = converter.GetApplicationUserInput(request, role.Id);
                userInput.JobTitle = request.FullName;
                var newUser = new ApplicationUser(userInput);
                var result = await authenticationService.Register(newUser, request.Password);
                if (result.Success)
                {
                    return Ok(new RegisterResponse()
                    {
                        Message = result.Message,
                        Email = newUser.Email,
                        Token = result.Data
                    });
                }
                return BadRequest(new FailedRegisterResponse()
                {
                    Message = result.Message,
                    Email = newUser.Email,
                });
            });
        }

        [HttpPost]
        [Route("Login")]
        public Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return TryCatchLogAsync(async () =>
            {
                var result = await authenticationService.Login(request.Email, request.Password);
                if (result.Success)
                {
                    return Ok(result.Data);
                }
                if (result.Data != null && !result.Data.EmailConfirmed)
                {
                    return BadRequest(new ResendOtpResponse()
                    {
                        Token = result.Data.Token,
                        Message = result.Message,
                        Email = request.Email,
                    });
                }
                return Unauthorized(result.Message);
            });
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        public Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request)
        {
            return TryCatchLogAsync(async () =>
            {
                var result = await authenticationService.ConfirmEmail(request.Email, request.Token, request.OTP);
                if (result.Success)
                {
                    var response = new ConfirmEmailResponse()
                    {
                        Message = result.Message,
                        Email = request.Email,
                    };
                    return Ok(response);
                }
                return BadRequest(result.Message);
            });
        }

        [HttpPost]
        [Route("ResendOTP")]
        public Task<IActionResult> ResendOTP(string email)
        {
            return TryCatchLogAsync(async () =>
            {
                var result = await authenticationService.ResendConfirmationEmailOTP(email);
                if (result.Success)
                {
                    var response = new ResendOtpResponse()
                    {
                        Token = result.Data,
                        Message = result.Message,
                        Email = email
                    };
                    return Ok(response);
                }
                return BadRequest(result.Message);
            });
        }

        [HttpPost]
        [Route("ForgetPassword")]
        public Task<IActionResult> ForgetPassword(string email)
        {
            return TryCatchLogAsync(async () =>
            {
                var result = await authenticationService.ForgetPassword(email);
                if (result.Success)
                {
                    var converter = new AuthenticationConverter(mapper);
                    var response = converter.GetForgetPasswordResponse(result, email);
                    return Ok(response);
                }
                var failedResponse = new FailedForgetPasswordResponse()
                {
                    Message = result.Message,
                    Email = email
                };
                return Ok(failedResponse);
            });
        }

        [HttpPost]
        [Route("ConfirmForgetPassword")]
        public Task<IActionResult> ConfirmForgetPassword(ConfirmForgetPassword request)
        {
            return TryCatchLogAsync(async () =>
            {
                if (!request.NewPassword.Equals(request.ConfirmNewPassword))
                    return BadRequest(new ConfirmForgetPasswordResponse()
                    {
                        Email = request.Email,
                        Message = ResponseMessages.PasswordDoesnotMatch
                    });
                var result = await authenticationService.ConfirmForgetPassword(request.Email, request.Token, request.NewPassword);
                if (result.Success)
                {
                    return Ok(new ConfirmForgetPasswordResponse()
                    {
                        Email = request.Email,
                        Message = result.Message
                    });
                }
                return Unauthorized(new ConfirmForgetPasswordResponse()
                {
                    Email = request.Email,
                    Message = result.Message
                });
            });
        }

        [HttpPost]
        [Route("ConfirmForgetPasswordOTP")]
        public Task<IActionResult> ConfirmForgetPasswordOTP(ConfirmForgetPasswordOTP confirmOtp)
        {
            return TryCatchLogAsync(async () =>
            {
                var result = await authenticationService.ConfirmForgetPasswordOTP(confirmOtp.Email, confirmOtp.OTP);
                if (result.Success)
                    return Ok(new ConfirmForgetPasswordOtpResponse()
                    {
                        Email = confirmOtp.Email,
                        Message = result.Message,
                        Token = result.Data
                    });
                return BadRequest(result.Message);
            });
        }
    }
}
