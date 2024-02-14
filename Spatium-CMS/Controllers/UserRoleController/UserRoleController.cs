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


namespace Spatium_CMS.Controllers.UserRoleController
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : CmsControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRoleController(UserManager<ApplicationUser> userManager, IMapper mapper, IUnitOfWork unitOfWork)
            : base(unitOfWork, mapper)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("GetAllRoles")]
        public Task<IActionResult> GetAllRoles([FromQuery] ViewRolePrams parms)
        {
            return TryCatchLogAsync(async () =>
            {
                var roles = await unitOfWork.RoleRepository.GetRolesAsync(parms);
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
                    Messege = $"Can not create role"
                });
            });
        }

        [HttpPut]
        [Route("UpdateRole")]
        [Authorize(Roles = "Super Admin")]
        public Task<IActionResult> UpdateRole(string roleId, RoleRequest request)
        {
            return TryCatchLogAsync(async () =>
            {
                var found = unitOfWork.RoleRepository.GetRoleByIdAsync(roleId);
                if (found != null)
                {
                    var converter = new RoleConverter(mapper);
                    var roleInput = converter.GetUpdatedRoleInput(request);
                    var updatedRole = new UserRole(roleInput);
                    var result = unitOfWork.RoleRepository.UpdateAsync(roleId, updatedRole);

                    if (result.IsCompletedSuccessfully)
                    {
                        await unitOfWork.SaveChangesAsync();
                        return Ok(new RoleResponse()
                        {
                            Messege = "the role is updated Successfully "
                        });
                    }
                    return BadRequest(new RoleResponse()
                    {
                        Messege = "can not be updated role"
                    });
                }
                return NotFound(new RoleResponse()
                {
                    Messege = $"Role not found"
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

