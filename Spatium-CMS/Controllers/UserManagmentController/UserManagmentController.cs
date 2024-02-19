using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domain.Interfaces;
using Domian.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.UserManagmentController.Request;
using Spatium_CMS.Filters;
using System.Net;
using Utilities.Enums;
using Utilities.Helpers;

namespace Spatium_CMS.Controllers.UserManagmentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagmentController : CmsControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly ISendMailService sendMailService;
        private readonly ILogger<UserManagmentController> logger;

        public UserManagmentController(IUnitOfWork unitOfWork, IMapper maper,
            UserManager<ApplicationUser> userManager, RoleManager<UserRole> roleManager
            , ISendMailService sendMailService, ILogger<UserManagmentController> logger)
            : base(unitOfWork, maper,logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.sendMailService = sendMailService;
            this.logger = logger;
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
                    var userId = GetUserId();
                    var loginUser = await userManager.FindByIdAsync(userId);

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
                        return Ok("User Created Successfully!");
                    }
                    return BadRequest(identityResult.Errors.Select(x => x.Description));
;
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
                    var LoginUser = await userManager.FindByIdAsync(userId);
                    var userUpdateInput = mapper.Map<ApplicationUserUpdateInput>(updateUserRequest);
                    LoginUser.Update(userUpdateInput);
                    await unitOfWork.SaveChangesAsync();
                    return Ok("Update Successfuly");
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
                        return Ok("Password Updated Successfuly");
                    }

                    return BadRequest(result.Errors.FirstOrDefault());
                }
                return BadRequest(ModelState);
            });
        }
    }
}
