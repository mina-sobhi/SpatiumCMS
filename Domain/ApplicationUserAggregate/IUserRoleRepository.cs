using Domain.ApplicationUserAggregate.Inputs;


namespace Domain.ApplicationUserAggregate
{
    public interface IUserRoleRepository
    {
        public Task<List<UserRole>> GetDefaultRoles(int blogId);
        public Task<UserRole> GetAssignRoleById(int blogId, string roleId);
        public Task<IReadOnlyList<ApplicationUser>> GetUsersByBlogIdAndRolePriority(int blogId, int priorityOfCurrent);
        public Task<List<UserRole>> SearchInRole(string CoulmnName, string Value);
        public Task<IReadOnlyList<UserRole>> GetRolesAsync(ViewRolePrams prams, int blogId);
        public Task<List<int>> GetRolePermissionIds(string roleId);
        //public void DeleteRolePermission(RolePermission rolePermission);
        public Task<UserRole> GetRoleByIdForUpdate(string roleId);
        public Task<UserRole> GetRoleByIdAsync(string roleId, int blogId);
        //public Task<bool> IsRoleAssignedToUserAsync(string roleId);
        //public Task<bool> DoesRoleHavePermissionsAsync(string roleId);
        //public Task CreatAsync(UserRole role);
        //public Task UpdateAsync(string roleId, UserRole role);

        public Task<List<UserModule>> GetModuleWithPermissions();
        public Task<List<ApplicationUser>> GetUsersInRoleAsync(string roleId);
        public Task<List<RolePermission>> GetRolePermissionsAsync(string roleId);
        public Task<List<ApplicationUser>> GetUserInRoleAsync(string roleId);

    }
}
