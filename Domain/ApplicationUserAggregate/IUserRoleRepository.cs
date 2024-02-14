using Domain.ApplicationUserAggregate.Inputs;


namespace Domain.ApplicationUserAggregate
{
    public interface IUserRoleRepository
    {
        public Task<IReadOnlyList<UserRole>> GetRolesAsync(ViewRolePrams prams);

        public Task<List<int>> GetRolePermissionIds(string roleId);
        
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
