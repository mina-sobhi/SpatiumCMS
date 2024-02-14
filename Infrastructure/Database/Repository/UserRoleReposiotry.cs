using Domain.ApplicationUserAggregate;
using Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;
using Domain.ApplicationUserAggregate.Inputs;

namespace Infrastructure.Database.Repository
{
    public class UserRoleReposiotry : RepositoryBase, IUserRoleRepository
    {
        public UserRoleReposiotry(SpatiumDbContent spatiumDbContent):base(spatiumDbContent)
        {
         
        }
        #region GetRoleDetailes
        public async Task<UserRole> GetRoleByIdAsync(string roleId)
        {
            return await SpatiumDbContent.Roles.FirstOrDefaultAsync(r => r.Id.Equals(roleId));
        }
        public async Task<IReadOnlyList<UserRole>> GetDefaultRoles()
        {
            return await SpatiumDbContent.Roles.Where(r => r.RoleOwnerId == null).ToListAsync();
        }
        public async Task<IReadOnlyList<UserRole>> GetRolesAsync(ViewRolePrams viewRoleParams)
        {
            if (viewRoleParams.IsActive == false)
            {
                return await SpatiumDbContent.Roles
               .Where(r => r.IsActive == viewRoleParams.IsActive)
                .Skip((viewRoleParams.PageIndex - 1) * viewRoleParams.PageSize)
               .Take(viewRoleParams.PageSize)
               .Include(ru => ru.ApplicationUsers)
               .ToListAsync();
            }
            else
            {
                return await SpatiumDbContent.Roles
                .Skip((viewRoleParams.PageIndex - 1) * viewRoleParams.PageSize)
               .Take(viewRoleParams.PageSize)
               .Include(ru => ru.ApplicationUsers)
               .ToListAsync();
            }
        }
        public async Task<List<UserModule>> GetModuleWithPermissions()
        {
            return await SpatiumDbContent.UserModules.Include(m => m.UserPermissions).ToListAsync();
        }
        public Task<List<int>> GetRolePermissionIds(string roleId)
        {
            return SpatiumDbContent.RolePermissions.Where(r => r.UserRoleId == roleId).Select(r => r.UserPermissionId).ToListAsync();
        } 
        #endregion 

        public async Task CreatAsync(UserRole role)
        {
            await SpatiumDbContent.Roles.AddAsync(role);
        }
        public async Task UpdateAsync(string roleId,UserRole role)
        {
            var found = await SpatiumDbContent.Roles.FirstOrDefaultAsync(r => r.Id.Equals(roleId));
            if (found != null)
            {
                 SpatiumDbContent.Roles.Update(found);
            }
        }

        #region Delete Role 
        public async Task<List<ApplicationUser>> GetUserInRoleAsync(string roleId)
        {
            return await SpatiumDbContent.Users.Where(ur => ur.RoleId == roleId).ToListAsync();
        }

        public async Task<List<RolePermission>> GetRolePermissionsAsync(string roleId)
        {
            return await SpatiumDbContent.RolePermissions.Where(rp=>rp.UserRoleId== roleId).ToListAsync();

        }
        #endregion
    }
}
