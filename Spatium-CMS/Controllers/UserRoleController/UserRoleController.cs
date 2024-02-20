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
                userResult.Unassign();
                await unitOfWork.SaveChangesAsync();
                return Ok(" User UnAssigne Succefuly ");
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
                var user = await _userManager.FindUserInBlogAsync(blogId, userId);
                if (user == null)
                    return BadRequest("User Not Found");

                var role = await unitOfWork.RoleRepository.GetAssignRoleById(blogId, roleId);
                if (role != null)
                {
                    user.AssigneToRole(roleId);
                    await unitOfWork.SaveChangesAsync();
                    return Ok("User Assigned Successfully");
                }

                return BadRequest();
                //var loginUser = await _userManager.FindByIdAsync(loginUserId);

                //if (user is null || role is null)
                //{
                //    return BadRequest(" Ivalid Prameter ");
                //}
                //var blogId = loginUser.BlogId;
                //var priority = loginUser.Role.Priority;
                //var users = await unitOfWork.RoleRepository.GetUsersByBlogIdAndRolePriority(blogId, priority);
                //if (users.SingleOrDefault(u => u.Id == user.ParentUserId) is null)
                //{
                //    return BadRequest("You Are Not Allow To Change This User");
                //}
                //user.AssigneToRole(RoleId);
                //await unitOfWork.SaveChangesAsync();
                //return Ok(" User Assigne To Role  Succefuly ");
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
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> GetDefaultRoles()
        {
            return TryCatchLogAsync(async () =>
            {
                var userId = GetUserId();
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest("User Not found!");
                var roles = await unitOfWork.RoleRepository.GetDefaultRoles(user.BlogId);
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
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                    return BadRequest("User Not found!");
                var roles = await unitOfWork.RoleRepository.GetRolesAsync(parms, user.BlogId);
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
                var blogId = GetBlogId();
                var role = await unitOfWork.RoleRepository.GetRoleByIdAsync(roleId, blogId);
                if (role == null)
                    return NotFound();
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
                var blogId = GetBlogId();
                var currentUser = await _userManager.FindByIdAsync(userId);
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

                //var result = unitOfWork.RoleRepository.CreatAsync(newRole);

                if (result.Succeeded)
                {
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
        public Task<IActionResult> UpdateRole(UpdateUserRoleRequest request)
        {
            return TryCatchLogAsync(async () =>
            {
                var role = await unitOfWork.RoleRepository.GetRoleByIdForUpdate(request.Id);
                if (role != null)
                {
                    var converter = new RoleConverter(mapper);
                    var updateRoleInput = mapper.Map<UpdateUserRoleInput>(request);
                    role.UpdateData(updateRoleInput);
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
            return TryCatchLogAsync(async () =>
            {
                var blogId = GetBlogId();
                var role = await unitOfWork.RoleRepository.GetRoleByIdAsync(roleId, blogId);
                if (role != null && role.RoleOwnerId == null)
                    return Unauthorized("This role cannot be deleted");
                role.Delete();
                await unitOfWork.SaveChangesAsync();
                return Ok(new
                {
                    Message = $"{role.Name} is Deleted Success!"
                });
            });
        }


    }

}
