using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.Interfaces;
using Domian.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Spatium_CMS.Controllers.AuthenticationController.Converter;
using Spatium_CMS.Controllers.AuthenticationController.Request;
using Spatium_CMS.Controllers.AuthenticationController.Response;
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

        public AuthenticationController(IAuthenticationService authenticationService, IMapper mapper, RoleManager<UserRole> roleManager ,IUnitOfWork unitOfWork,UserManager<ApplicationUser> userManager) :base(unitOfWork ,mapper)
        {
            this.authenticationService = authenticationService;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpGet]
        [Route("ChangeUserActivation")]
        [Authorize]
        //[PermissionFilter(PermissionsEnum.UpdateUser)]
        public Task<IActionResult> ChangeUserActivation(string id)
        {
            return TryCatchLogAsync(async () =>
            {
                //var email = User.FindFirstValue(ClaimTypes.Email);
                //var currentuser = await userManager.FindByEmailAsync(email);
                //var userId = currentuser.Id;
                await authenticationService.ChangeUserActivation(id);
                await unitOfWork.SaveChangesAsync();
                return Ok("the Activation is Changed");
            });
        }

        [HttpGet]
        [Authorize]
        [Route("GetUserDetails")]
        public Task<IActionResult> GetUserDetails()
        {
            return TryCatchLogAsync(async () =>
            {
                var email = GetUserId();
                var currentuser = await userManager.FindByEmailAsync(email);
                var userId = currentuser.Id;
                var userdetailes=await authenticationService.GetUserDetailes(userId);
                var detailesResult=mapper.Map<ViewUserProfileResult>(userdetailes);

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
                var roleId = await roleManager.Roles.Select(x => x.Id).FirstOrDefaultAsync();
                var userInput = converter.GetApplicationUserInput(request, roleId);
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
                var result = await authenticationService.ConfirmOTP(request.Email, request.Token, request.OTP);
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
                var result =await authenticationService.ConfirmForgetPassword(request.Email, request.Token, request.NewPassword);
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
    }
}
