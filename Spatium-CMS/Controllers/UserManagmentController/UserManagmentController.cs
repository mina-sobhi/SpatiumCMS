using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domain.Interfaces;
using Domian.Interfaces;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Spatium_CMS.Controllers.BlogsController.Response;
using Spatium_CMS.Controllers.UserManagmentController.Request;
using Spatium_CMS.Filters;
using System.Net;
using Utilities.Enums;
using Utilities.Helpers;
using Utilities.Results;

namespace Spatium_CMS.Controllers.UserManagmentController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserManagmentController : CmsControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper maper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<UserRole> roleManager;
        private readonly ISendMailService sendMailService;

        public UserManagmentController(IUnitOfWork unitOfWork , IMapper maper ,
            UserManager<ApplicationUser> userManager , RoleManager<UserRole> roleManager 
            , ISendMailService sendMailService) 
            : base(unitOfWork, maper) 
        {
            this.unitOfWork = unitOfWork;
            this.maper = maper;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.sendMailService = sendMailService;
        }

        [HttpPost]
        [Route("CreateUser")]
        [Authorize]
        [PermissionFilter(PermissionsEnum.CreateUser)]
        public Task<IActionResult> CreateUser(CreateUserRequest createUserRequest)
        {
            return TryCatchLogAsync(async () =>
            {

                if(ModelState.IsValid)
                {
                    // get Current User
                    var userId = GetUserId();
                    var LoginUser = await userManager.FindByIdAsync(userId);
                    // check user is superAdmin 
                    //var FlagUserInRole = LoginUser.Role.Name == "Super Admin";
                    //if (!FlagUserInRole)
                    //{
                    //    return Unauthorized("You Are Not Allow To Create User ");
                    //}
                    // Create New User 
                    var applicationUserInput = new ApplicationUserInput();
                    applicationUserInput.FullName = createUserRequest.FullName;
                    applicationUserInput.ProfileImagePath = createUserRequest.ProfileImagePath;
                    applicationUserInput.Email  =   createUserRequest.Email;
                    applicationUserInput.RoleId = createUserRequest.RoleId;
                    applicationUserInput.JobTitle = "UnAssigned";
                    applicationUserInput.ParentUserId = LoginUser.Id;
                    var applicationUser = new ApplicationUser(applicationUserInput);
                    var user = await userManager.FindByEmailAsync(applicationUser.Email);
                    if (user != null)
                    {
                        return BadRequest($"Email {applicationUser.Email} Already Exists ");
                    }
                    var identityResult = await userManager.CreateAsync(applicationUser, createUserRequest.Password);

                    if(identityResult.Succeeded)
                    {
                        var token = await userManager.GenerateEmailConfirmationTokenAsync(applicationUser);
                        var encodedToken = WebUtility.UrlEncode(token);
                        applicationUser.ChangeOTP(OTPGenerator.GenerateOTP());
                        await unitOfWork.SaveChangesAsync();
                      
                        string MailBody = $"<h1> Your OTP is {applicationUser.OTP} </h1> <br/> <a style='padding:10px;background-color:blue;color:#fff ;text-decoration:none' href ='https://localhost:7109/api/Authentication/ConfirmEmail?email={applicationUser.Email}&token={encodedToken}'> Verification Email </a>";
                        var flag = await sendMailService
                        .SendMail(applicationUser.Email, "Spatium CMS Verification Email!", MailBody);
                        if(flag)
                        {
                            return Ok("User Created Succefuly ");
                        }
                        return BadRequest("error In Mail Sever ");
                    }
                    return BadRequest("Error In DB")
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
