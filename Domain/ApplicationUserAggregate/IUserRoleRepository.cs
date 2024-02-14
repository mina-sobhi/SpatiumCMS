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

        #region Role Deleation
        //public Task DeleteRoleAsync(string roleId);
        public Task<bool> IsRoleAssignedToUserAsync(string roleId);
        public Task<bool> DoesRoleHavePermissionsAsync(string roleId);
        public Task<ApplicationUser> GetUserInRoleAsync(string roleId); 
        #endregion

    }
}
