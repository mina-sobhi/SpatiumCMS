using Domain.ApplicationUserAggregate.Inputs;


namespace Domain.ApplicationUserAggregate
{
    public interface IUserRoleRepository
    {
        #region Hesham 
        Task<IReadOnlyList<UserRole>> GetDefaultRoles(int blogId);
        Task<IReadOnlyList<ApplicationUser>> GetUsersByBlogIdAndRolePriority(int blogId, int priorityOfCurrent);
        Task<List<UserRole>> SearchInRole(string CoulmnName, string Value);
        #endregion
        public Task<IReadOnlyList<UserRole>> GetRolesAsync(ViewRolePrams prams,int blogId);
        public Task<List<int>> GetRolePermissionIds(string roleId);
        public void DeleteRolePermission (RolePermission rolePermission);
        public Task<UserRole> GetRoleByIdForUpdate(string roleId);
        public Task<UserRole> GetRoleByIdAsync(string roleId);
        public Task CreatAsync(UserRole role);
        public Task UpdateAsync(string roleId, UserRole role);

        public Task<IReadOnlyList<UserRole>> GetDefaultRoles();

        public Task<List<UserModule>> GetModuleWithPermissions();

        #region Role Deleation
        public Task<List<RolePermission>> GetRolePermissionsAsync(string roleId);
        public Task<List<ApplicationUser>> GetUserInRoleAsync(string roleId); 
        #endregion

    }
}
