using Domain.ApplicationUserAggregate;
using Infrastructure.Database.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Domain.ApplicationUserAggregate.Inputs;
using Infrastructure.Database.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.Repository
{
    public class UserRoleReposiotry : RepositoryBase, IUserRoleRepository
    {
        public UserRoleReposiotry(SpatiumDbContent spatiumDbContent):base(spatiumDbContent)
        {
         
        }


        public async Task<UserRole> GetRoleByIdAsync(string roleId)
        {
            return await SpatiumDbContent.Roles.FirstOrDefaultAsync(r => r.Id.Equals(roleId));
        }

        public async Task<IReadOnlyList<UserRole>> GetRolesAsync()
        {
            return await SpatiumDbContent.Roles.ToListAsync();
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

        public Task<List<int>> GetRolePermissionIds(string roleId)
        {
            return SpatiumDbContent.RolePermissions.Where(r=>r.UserRoleId== roleId).Select(r=>r.UserPermissionId).ToListAsync();
        }
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
        public async Task DeleteRoleAsync(string roleId)
        {
            //deleted related user
            var user = await GetUserInRoleAsync(roleId);
            SpatiumDbContent.Users.Remove(user);

            //deleted related roles in rolepermissions
            var rolePermissions = await SpatiumDbContent.RolePermissions.Where(rp => rp.UserRoleId == roleId).ToListAsync();
            SpatiumDbContent.RolePermissions.RemoveRange(rolePermissions);


            //deleted the role from the table of role 
            var role = await SpatiumDbContent.Roles.FirstOrDefaultAsync(r=>r.Id==roleId);
            SpatiumDbContent.Roles.Remove(role);
        }

        #region CHECK THE ASSOCCIATIONS WITH ROLE 
        public async Task<ApplicationUser> GetUserInRoleAsync(string roleId)
        {
            return await SpatiumDbContent.Users.FirstOrDefaultAsync(ur => ur.RoleId == roleId);
        }
        public async Task<bool> IsRoleAssignedToUserAsync(string roleId)
        {
            return await SpatiumDbContent.UserRoles.AnyAsync(ur => ur.RoleId == roleId);
        }
        public async Task<bool> DoesRoleHavePermissionsAsync(string roleId)
        {
            return await SpatiumDbContent.RolePermissions.AnyAsync(rp => rp.UserRoleId == roleId);
        }
        #endregion

        #endregion
    }
}
