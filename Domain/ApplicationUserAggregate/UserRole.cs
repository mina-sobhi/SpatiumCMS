using Domain.ApplicationUserAggregate.Inputs;
using Microsoft.AspNetCore.Identity;

namespace Domain.ApplicationUserAggregate
{
    public class UserRole : IdentityRole
    {
        #region Properties
        public string IconPath { get; private set; }
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public string RoleOwnerId { get; private set; }
        public int Priority { get; private set; }
        #endregion

        #region Navigational Properties
        public virtual ApplicationUser RoleOwner { get; private set; }
        #endregion

        #region Virtual List
        private readonly List<RolePermission> _rolePermission = new List<RolePermission>();
        public virtual IReadOnlyList<RolePermission> RolePermission => _rolePermission;

        private readonly List<ApplicationUser> _applicationUsers = new List<ApplicationUser>();
        public virtual IReadOnlyList<ApplicationUser> ApplicationUsers => _applicationUsers.ToList();
        #endregion

        #region CTORS
        public UserRole() { }

        public UserRole(UserRoleInput userRoleInput)
        {
            Name = userRoleInput.Name;
            IconPath = userRoleInput.IconPath;
            Description = userRoleInput.Description;
            IsActive = userRoleInput.IsActive;
            RoleOwnerId = userRoleInput.RoleOwnerId;
            Priority = userRoleInput.RoleOwnerPriority++;
            foreach (var permissionId in userRoleInput.UserPermissionId)
            {
                var newrolepermission = new RolePermission(this.Id, permissionId);
                _rolePermission.Add(newrolepermission);
            }
        }
        #endregion

    }
}
