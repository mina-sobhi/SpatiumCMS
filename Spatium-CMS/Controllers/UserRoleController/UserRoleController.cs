using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domian.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Spatium_CMS.Controllers.UserRoleController.Converter;
using Spatium_CMS.Controllers.UserRoleController.Request;
using Spatium_CMS.Controllers.UserRoleController.Response;
using Infrastructure.Extensions;
using Utilities.Exceptions;
using System.Data;
using Utilities.Results;

namespace Spatium_CMS.Controllers.UserRoleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : CmsControllerBase
    {
        private readonly RoleManager<UserRole> roleManager;


        public UserRoleController(ILogger<UserRoleController> logger, UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork, RoleManager<UserRole> roleManager)
            : base(unitOfWork, mapper, logger, userManager)
        {
            this.roleManager = roleManager;
        }

        [HttpGet]
        [Route("Unassign")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> Unassign(string userId)
        {
            return TryCatchLogAsync(async () =>
            {
                string parentUserId = GetUserId();
                var blogId = GetBlogId();
                var userResult = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                userResult.Unassign();
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = ResponseMessages.UserUnassignedSuccessfully,
                    Success = true
                };
                return Ok(response);
            });
        }

        [HttpGet]
        [Route("Assign")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> Assign(string userId, string roleId)
        {
            return TryCatchLogAsync(async () =>
            {
                var loginUserId = GetUserId();
                var blogId = GetBlogId();
                var user = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);

                var role = await unitOfWork.RoleRepository.GetAssignRoleById(blogId, roleId) ?? throw new SpatiumException(ResponseMessages.InvalidRole);

                user.AssigneToRole(roleId);
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = ResponseMessages.UserAssignedSuccessfully,
                    Success = true,
                };
                return Ok(response);
            });
        }


        [HttpGet]
        [Route("SearchInRole")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> SearchInRole(string CoulmnName, string Value)
        {
            return TryCatchLogAsync(async () =>
            {
                var result = await unitOfWork.RoleRepository.SearchInRole(CoulmnName, Value);
                //if (result.Count() <= 0)
                //{
                //    BadRequest("No Data");
                //}
                var response = mapper.Map<List<ViewRoles>>(result);
                return Ok(response);
            });
        }

        [HttpGet]
        [Route("GetDefaultRoles")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> GetDefaultRoles()
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var roles = await unitOfWork.RoleRepository.GetDefaultRoles(blogId);
                var roleresualt = mapper.Map<List<ViewRoles>>(roles);
                return Ok(roleresualt);
            });
        }

        [HttpGet]
        [Route("GetAllRoles")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> GetAllRoles([FromQuery] ViewRolePrams parms)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var blogId = GetBlogId();
                var roles = await unitOfWork.RoleRepository.GetRolesAsync(parms, blogId) ?? throw new SpatiumException(ResponseMessages.InvalidRole);
                var roleResponse = mapper.Map<List<ViewRoles>>(roles);
                return Ok(roleResponse);
            });
        }

        [HttpGet]
        [Route("GetRoleById")]

        public Task<IActionResult> GetById(string roleId)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var role = await unitOfWork.RoleRepository.GetRoleByIdAsync(roleId, blogId) ?? throw new SpatiumException(ResponseMessages.InvalidRole);
                var roleDto = mapper.Map<ViewRoles>(role);
                return Ok(roleDto);
            });
        }

        [HttpGet]
        [Route("GetModuleWithPermissions")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> GetModuleWithPermissions()
        {
            return TryCatchLogAsync(async () =>
            {
                var module = await unitOfWork.RoleRepository.GetModuleWithPermissions() ?? throw new SpatiumException(ResponseMessages.InvalidRoleModule);
                var moduleResualt = mapper.Map<List<ViewModule>>(module);
                return Ok(moduleResualt);
            });
        }

        [HttpPost]
        [Route("CreateRole")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> CreateRole(RoleRequest request)
        {
            return TryCatchLogAsync(async () =>
            {

                var userId = GetUserId();
                var blogId = GetBlogId();
                var currentUser = await userManager.FindUserInBlogAsync(blogId, userId) ?? throw new SpatiumException(ResponseMessages.UserNotFound);
                var converter = new RoleConverter(mapper);
                UserRoleInput roleInput = new UserRoleInput();
                if (!currentUser.ParentUserId.IsNullOrEmpty())
                {
                    roleInput = converter.CreateUserRoleInput(request, currentUser.ParentUserId);
                    roleInput.RoleOwnerPriority = currentUser.Role.Priority;
                    roleInput.BlogId = blogId;
                }
                else
                {
                    roleInput = converter.CreateUserRoleInput(request, currentUser.Id);
                    roleInput.RoleOwnerPriority = currentUser.Role.Priority;
                    roleInput.BlogId = blogId;
                }
                var newRole = new UserRole(roleInput);
                var result = await roleManager.CreateAsync(newRole);

                if (result.Succeeded)
                {
                    var response = new SpatiumResponse()
                    {
                        Message = ResponseMessages.RoleCreatedSuccessfully,
                        Success = true
                    };
                    return Ok(result);
                }
                throw new SpatiumException(result.Errors.Select(x => x.Description).ToArray());
            });
        }


        [HttpPut]
        [Route("UpdateRole")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> UpdateRole(UpdateUserRoleRequest request)
        {
            return TryCatchLogAsync(async () =>
            {
                var role = await unitOfWork.RoleRepository.GetRoleByIdForUpdate(request.Id) ?? throw new SpatiumException(ResponseMessages.InvalidRole); ;
                var converter = new RoleConverter(mapper);
                var updateRoleInput = mapper.Map<UpdateUserRoleInput>(request);
                role.UpdateData(updateRoleInput);
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = ResponseMessages.RoleUpdatedSuccessfully,
                    Success = true
                };
                return Ok(response);
            });
        }

        [HttpDelete]
        [Route("DeleteRole")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> DeleteRole(string roleId)
        {
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var role = await unitOfWork.RoleRepository.GetRoleByIdAsync(roleId, blogId) ?? throw new SpatiumException(ResponseMessages.InvalidRole);
                if (role.RoleOwnerId == null)
                    throw new SpatiumException($"{role.Name} is a Default Role, It can't be Deleted");
                role.Delete();
                await unitOfWork.SaveChangesAsync();
                var response = new SpatiumResponse()
                {
                    Message = $"{role.Name} is Deleted Success!",
                    Success = true,
                };
                return Ok(response);
            });
        }

        [HttpGet]
        [Route("GetRoleIcons")]
        [Authorize]
        public Task<IActionResult> GetRoleIcons()
        {
            return TryCatchLogAsync(async () =>
            {
                var icons = await unitOfWork.RoleRepository.GetRoleIconsAsync() ?? throw new SpatiumException(ResponseMessages.RoleIconsNotFound);
                var result = mapper.Map<List<RoleIconRespones>>(icons);
                return Ok(result);
            });
        }
    }

}
