using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domain.Base;
using Domain.Interfaces;
using Domian.Interfaces;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.AuthenticationController.Response;
using Spatium_CMS.Controllers.UserManagmentController.Request;
using Spatium_CMS.Controllers.UserManagmentController.Response;
using Spatium_CMS.Filters;
using System.Net;
using Utilities.Enums;
using Utilities.Exceptions;
using Utilities.Extensions;
using Utilities.Helpers;
using Utilities.Results;

namespace Spatium_CMS.Controllers.UserManagmentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagmentController : CmsControllerBase
    {
        private readonly ISendMailService sendMailService;
        private readonly IConfiguration configuration;
        private readonly IAuthenticationService authenticationService;

        public UserManagmentController(IUnitOfWork unitOfWork, IMapper maper,
            UserManager<ApplicationUser> userManager, ISendMailService sendMailService, ILogger<UserManagmentController> logger, IAuthenticationService authenticationService)
            : base(unitOfWork, maper, logger, userManager)
        {
            this.sendMailService = sendMailService;
            this.authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("CreateUser")]
        [Authorize(Roles = "Super Admin")]
        [PermissionFilter(PermissionsEnum.CreateUser)]
        public Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if(ModelState.IsValid)
                {
                    if (createUserRequest.RoleId == MainRolesIdsEnum.SuperAdmin.GetDescription())
                        throw new SpatiumException(ResponseMessages.InvalidRole);
                    var userId = GetUserId();
                    var loginUser = await userManager.FindByIdAsync(userId)??throw new SpatiumException(ResponseMessages.UserNotFound);

                    var applicationUserInput = new ApplicationUserInput();
                    applicationUserInput.FullName = createUserRequest.FullName;
                    applicationUserInput.ProfileImagePath = createUserRequest.ProfileImagePath;
                    applicationUserInput.Email  =   createUserRequest.Email;
                    applicationUserInput.RoleId = createUserRequest.RoleId;
                    applicationUserInput.JobTitle = "UnAssigned";
                    applicationUserInput.ParentUserId = loginUser.Id;
                    applicationUserInput.ParentBlogId = loginUser.BlogId;
                    var applicationUser = new ApplicationUser(applicationUserInput);

                    var identityResult = await userManager.CreateAsync(applicationUser, createUserRequest.Password);

                    if(identityResult.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                        var encodedToken = WebUtility.UrlEncode(token);
                        applicationUser.ChangeOTP(OTPGenerator.GenerateOTP());
                        await unitOfWork.SaveChangesAsync();
                      
                        string MailBody = $"<h1> Your OTP is {applicationUser.OTP} </h1> <br/> <a style='padding:10px;background-color:blue;color:#fff ;text-decoration:none' href ='https://localhost:7109/api/Authentication/ConfirmEmail?email={applicationUser.Email}&token={encodedToken}'> Verification Email </a>";
                        await sendMailService.SendMail(applicationUser.Email, "Spatium CMS Verification Email!", MailBody);

                        var response = new SpatiumResponse()
                        {
                            Message=ResponseMessages.UserCreatedSuccessfully,
                            Success=true
                        };
                        return Ok(response);
                    }
                    throw new SpatiumException(identityResult.Errors.Select(x => x.Description).ToArray());
                }
                return BadRequest(ModelState);
            });
        }

        [HttpPut]
        [Route("UpdateUser")]
        [Authorize]
        public Task<IActionResult> UpdateUser(UpdateUserRequest updateUserRequest)
        {
            return TryCatchLogAsync(async () =>
            {
                if (ModelState.IsValid)
                {
                    var userId = GetUserId();
                    var user = await userManager.FindByIdAsync(userId)?? throw new SpatiumException(ResponseMessages.UserNotFound);
                    var userUpdateInput = mapper.Map<ApplicationUserUpdateInput>(updateUserRequest);
                    user.Update(userUpdateInput);
                    await unitOfWork.SaveChangesAsync();
                    var response = new SpatiumResponse()
                    {
                        Message = ResponseMessages.UserUpdatedSuccessfully,
                        Success = true
                    };
                    return Ok(response);
                }
                return BadRequest(ModelState);
            });
        }


        [HttpPut]
        [Route("UpdateUserPassword")]
        [Authorize]
        public Task<IActionResult> UpdateUserPassword(UpdateUserPaswordRequest updateUserPasswordRequest)
        {
            return TryCatchLogAsync(async () =>
            {

                if (ModelState.IsValid)
                {
                    var userId = GetUserId();
                    var LoginUser = await userManager.FindByIdAsync(userId);
                    
                    IdentityResult result = await userManager.ChangePasswordAsync(LoginUser,updateUserPasswordRequest.CurrenetPassword ,updateUserPasswordRequest.NewPassword);
                   
                    if(result.Succeeded)
                    {
                        await unitOfWork.SaveChangesAsync();
                        var response = new SpatiumResponse()
                        {
                            Message = ResponseMessages.PasswordChangedSuccessfully,
                            Success = true
                        };
                        return Ok(response);
                    }
                    throw new SpatiumException(result.Errors.Select(x=>x.Description).ToArray());
                }
                return BadRequest(ModelState);
            });
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
                var currentuser = await userManager.FindUserInBlogByIdIncludingRole(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var detailesResult = mapper.Map<ViewUserProfileResult>(currentuser);
                return Ok(detailesResult);
            });
        }
        [HttpGet]
        [Route("GetUserAllUsers")]
        [Authorize]
        public Task<IActionResult> GetUserAllUsers([FromQuery]GetEntitiyParams entityParams)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var users = await userManager.FindUsersInBlogIncludingRole(blogId, entityParams) ?? throw new SpatiumException("there are not users !!");
                var detailesResult = mapper.Map<List<ViewUsersResponse>>(users);
                return Ok(detailesResult);
            });
        }
        [HttpGet]
        [Route("GetUsersAnalytics")]
        [Authorize]
        public Task<IActionResult> GetUsersAnalytics()
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var detailesResult = await userManager.GetBlogUersAnalytics(blogId) ?? throw new SpatiumException("there are not users !!");
                return Ok(detailesResult);
            });
        }
        [HttpGet]
        [Route("GetUserActivity")]
        [Authorize]
        public Task<IActionResult> GetUserActivity()
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var activity = await unitOfWork.RoleRepository.GetActivityLog(userId);
                var response = new List<ActivityResponse>();
                foreach (var item in activity)
                {
                    response.Add(new ActivityResponse()
                    {
                        Content = item.Content,
                        IconPath = configuration.GetSection("ApiBaseUrl").Value +"/" + item.LogIcon.Path,
                        Date = item.CreationDate
                    });
                }
                return Ok(response);
            });
        }
    }
}
