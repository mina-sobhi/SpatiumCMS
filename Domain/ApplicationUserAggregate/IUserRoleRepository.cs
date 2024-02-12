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

        #region Role Deleation
        //public Task DeleteRoleAsync(string roleId);
        public Task<bool> IsRoleAssignedToUserAsync(string roleId);
        public Task<bool> DoesRoleHavePermissionsAsync(string roleId);
        public Task<ApplicationUser> GetUserInRoleAsync(string roleId); 
        #endregion

    }
}
