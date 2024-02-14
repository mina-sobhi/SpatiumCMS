using AutoMapper;
using Domain.ApplicationUserAggregate;
using Domain.ApplicationUserAggregate.Inputs;
using Domian.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.IdentityModel.Tokens;
using Spatium_CMS.Controllers.UserRoleController.Converter;
using Spatium_CMS.Controllers.UserRoleController.Request;
using Spatium_CMS.Controllers.UserRoleController.Response;
using Infrastructure.Extensions;
using System.Data;
using System.Security.Claims;
using Spatium_CMS.Filters;

namespace Spatium_CMS.Controllers.UserRoleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : CmsControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<UserRole> roleManager;


        public UserRoleController(ILogger<UserRoleController> logger, UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork, RoleManager<UserRole> roleManager)
            : base(unitOfWork, mapper, logger)
        {
            _userManager = userManager;
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
                var parentUser = await _userManager.FindByIdAsync(parentUserId);
                var userResult = await _userManager.FindUserInBlogAsync(parentUser.BlogId, userId);
                if (userResult is null)
                {
                    return BadRequest("Ivalid User Id");
                }
                userResult.UnAssigne();
                await unitOfWork.SaveChangesAsync();
                return Ok(" User UnAssigne Succefuly ");
            });
        }

        [HttpGet]
        [Route("AssigneUserToSpacificRole")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> AssigneUserToSpacificRole(string userId, string RoleId)
        {
            return TryCatchLogAsync(async () =>
            {
                var user = await _userManager.FindByIdAsync(userId);
                var role = await roleManager.FindByIdAsync(RoleId);
                var loginUserId = GetUserId();
                var loginUser = await _userManager.FindByIdAsync(loginUserId);

                if (user is null || role is null)
                {
                    return BadRequest(" Ivalid Prameter ");
                }
                var blogId = loginUser.BlogId;
                var priority = loginUser.Role.Priority;
                var users = await unitOfWork.RoleRepository.GetUsersByBlogIdAndRolePriority(blogId, priority);
                if (users.SingleOrDefault(u => u.Id == user.ParentUserId) is null)
                {
                    return BadRequest("You Are Not Allow To Change This User");
                }
                user.AssigneToRole(RoleId);
                await unitOfWork.SaveChangesAsync();
                return Ok(" User Assigne To Role  Succefuly ");
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
                if (result.Count() <= 0)
                {
                    BadRequest("No Data");
                }
                var response = mapper.Map<List<ViewRoles>>(result);
                return Ok(response);
            });
        }

        [HttpGet]
        [Route("GetDefaultRoles")]
        [Authorize]
        public Task<IActionResult> GetDefaultRoles()
        {
            return TryCatchLogAsync(async () =>
            {
                var userId=GetUserId();
                var user=await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest("User Not found!");
                var roles = await unitOfWork.RoleRepository.GetDefaultRoles(user.BlogId);
                var roleresualt = mapper.Map<List<ViewRoles>>(roles);
                return Ok(roleresualt);
            });
        }
        [HttpGet]
        [Route("GetAllRoles")]
        public Task<IActionResult> GetAllRoles([FromQuery] ViewRolePrams parms)
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest("User Not found!");
                var roles = await unitOfWork.RoleRepository.GetRolesAsync(parms,user.BlogId);
                var roleresualt = mapper.Map<List<ViewRoles>>(roles);
                return Ok(roleresualt);
            });
        }

        [HttpGet]
        [Route("GetRoleById")]
        public Task<IActionResult> GetById(string roleId)
        {
            return TryCatchLogAsync(async () =>
            {
                var role = await unitOfWork.RoleRepository.GetRoleByIdAsync(roleId);
                if (role == null)
                    return NotFound();
                var roleDto = mapper.Map<ViewRoles>(role);
                return Ok(roleDto);
            });
        }

        [HttpGet]
        [Route("GetDefaultRoles")]
        public Task<IActionResult> GetDefaultRoles()
        {
            return TryCatchLogAsync(async () =>
            {
                var roles = await unitOfWork.RoleRepository.GetDefaultRoles();
                if (roles == null)
                    return NotFound("not found roles");
                var roleresualt = mapper.Map<List<ViewRoles>>(roles);
                return Ok(roleresualt);
            });
        }

        [HttpGet]
        [Route("GetModuleWithPermissions")]
        public Task<IActionResult> GetModuleWithPermissions()
        {
            return TryCatchLogAsync(async () =>
            {
                var module = await unitOfWork.RoleRepository.GetModuleWithPermissions();
                if (module == null)
                    return NotFound("not found Module");
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
                var currentUser = await _userManager.FindByIdAsync(userId);
                var converter = new RoleConverter(mapper);
                UserRoleInput roleInput = new UserRoleInput();

                if (!currentUser.ParentUserId.IsNullOrEmpty())
                {
                    roleInput = converter.CreateUserRoleInput(request, currentUser.ParentUserId);
                    roleInput.RoleOwnerPriority = currentUser.Role.Priority;
                }
                else
                {
                    roleInput = converter.CreateUserRoleInput(request, currentUser.Id);
                    roleInput.RoleOwnerPriority = currentUser.Role.Priority;
                }

                var newRole = new UserRole(roleInput);
                var result = unitOfWork.RoleRepository.CreatAsync(newRole);


                if (!currentUser.ParentUserId.IsNullOrEmpty())
                {
                    roleInput = converter.CreateUserRoleInput(request, currentUser.ParentUserId);
                    roleInput.RoleOwnerPriority = currentUser.Role.Priority;
                }
                else
                {
                    roleInput = converter.CreateUserRoleInput(request, currentUser.Id);
                    roleInput.RoleOwnerPriority = currentUser.Role.Priority;
                }

                var newRole = new UserRole(roleInput);
                var result = unitOfWork.RoleRepository.CreatAsync(newRole);

                if (result.IsCompletedSuccessfully)
                {
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new RoleResponse()
                    {
                        Messege = $"the role {newRole.Name} is created Successfully "
                    });
                }
                return BadRequest(new RoleResponse()
                {

                    Messege = $"can not create role"
                });
            });
        }


        [HttpPut]
        [Route("UpdateRole")]
        [Authorize(Roles = "Super Admin")]

        [Authorize(Roles ="Super Admin")]
        public Task<IActionResult> UpdateRole(UpdateUserRoleRequest request)
        {
            return TryCatchLogAsync(async () =>
            {
                var role = await unitOfWork.RoleRepository.GetRoleByIdForUpdate(request.Id);
                if (role != null)
                {
                    var converter = new RoleConverter(mapper);
                    var updateRoleInput=mapper.Map<UpdateUserRoleInput>(request);
                    foreach(var rp in role.RolePermission)
                    {
                        unitOfWork.RoleRepository.DeleteRolePermission(rp);
                    }
                    await unitOfWork.SaveChangesAsync();
                    role.UpdateData(updateRoleInput);
                    role.AddPermissions(request.PermissionIds);
                    await unitOfWork.SaveChangesAsync();
                    return Ok(new RoleResponse()
                    {
                        Messege = $"Role {role.Name} updated successfully!"

                    });
                }
                return NotFound(new RoleResponse()
                {
                    Messege = $"Invalid Role"
                });
            });
        }

        [HttpDelete]
        [Route("DeleteRole")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> DeleteRole(string roleId)
        {
            return TryCatchLogAsync(async () =>{

            var roleAssignedToUsers = await unitOfWork.RoleRepository.IsRoleAssignedToUserAsync(roleId);
            var roleHasPermissions = await
             unitOfWork.RoleRepository.DoesRoleHavePermissionsAsync(roleId);
            if (roleAssignedToUsers || roleHasPermissions)
            {
                // await unitOfWork.RoleRepository.DeleteRoleAsync(roleId);
                return NoContent();
            }
            else
            {
                var userId = GetUserId();
                var currentUser = await _userManager.FindByIdAsync(userId);
                var role = await unitOfWork.RoleRepository.GetRoleByIdAsync(roleId);

                if (role.RoleOwner != null && currentUser.Id==role.RoleOwnerId)
                {
                    var roleUsers = await unitOfWork.RoleRepository.GetUserInRoleAsync(roleId);
                    var rolePermissions=await unitOfWork.RoleRepository.GetRolePermissionsAsync(roleId);
                    foreach (var user in roleUsers)
                    {
                        //implement un Assigned role to users
                    }
                    foreach (var rolePermission in rolePermissions)
                        rolePermission.Deleted();
                }
                return BadRequest(new RoleResponse()
                {
                    Messege = "Cannot delete role. It is Defualt Role or you do not have permission for it."
                });
            });
        }
    }
}

