using Domain.ApplicationUserAggregate.Inputs;
using Domain.BlogsAggregate;
using Domain.LookupsAggregate;
using Microsoft.AspNetCore.Identity;
using Utilities.Exceptions;
using Utilities.Results;

namespace Domain.ApplicationUserAggregate
{
    public class UserRole : IdentityRole
    {
        #region Properties
        public string Description { get; private set; }
        public bool IsActive { get; private set; }
        public string RoleOwnerId { get; private set; }
        public int Priority { get; private set; }
        public bool IsDeleted { get; private set; }
        public string Color { get; private set; } 
        public int? BlogId { get; private set; }
        public int? RoleIconId { get; private set; }


        #endregion

        #region Navigational Properties
        public virtual ApplicationUser RoleOwner { get; private set; }
        public virtual Blog Blog { get; private set; }
        public virtual RoleIcon RoleIcon { get; private set; }
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
            Description = userRoleInput.Description;
            IsActive = userRoleInput.IsActive;
            RoleOwnerId = userRoleInput.RoleOwnerId;
            Priority = ++userRoleInput.RoleOwnerPriority;
            RoleIconId = userRoleInput.RoleIconId;
            Color = userRoleInput.Color;
            this.IsDeleted = false;
            BlogId = userRoleInput.BlogId ?? throw new SpatiumException(ResponseMessages.BlogIdCannotBeNull);
            foreach (var permissionId in userRoleInput.UserPermissionId)
            {
                var newrolepermission = new RolePermission(this.Id, permissionId);
                _rolePermission.Add(newrolepermission);
            }
        }
        #endregion

        public void Delete()
        {
            this.IsDeleted = true;
            foreach (var user in _applicationUsers)
                user.Unassign();
        }
        public void ClearPermissions()
        {
            _rolePermission.Clear();
        }
        public void UpdateData(UpdateUserRoleInput updateInput)
        {
            Description = updateInput.Description;
             
            //change the state of old permission 
            var oldPermissions = _rolePermission.Select(x => x.UserPermissionId).Except(updateInput.PermissionIds);
            foreach (var perm in oldPermissions)
            {
                //reset the old permissions to be deleted 
                var permission = _rolePermission.FirstOrDefault(p => p.UserPermissionId == perm);
                if (!permission.IsDeleted)
                    permission.Delete();
            }
            //change the state of current permission
            var CommonPermissions = _rolePermission.Select(x => x.UserPermissionId).Intersect(updateInput.PermissionIds);
            foreach (var perm in CommonPermissions)
            {
                //reset the deleted permissions
                var permission = _rolePermission.FirstOrDefault(p => p.UserPermissionId == perm);
                if (permission.IsDeleted)
                    permission.Delete();
            }
            //add new permissions 
            var newPermissions = updateInput.PermissionIds.Except(_rolePermission.Select(x => x.UserPermissionId));
            foreach (var newPermissionId in newPermissions)
            {
                _rolePermission.Add(new RolePermission(Id, newPermissionId));
            }
        }
    }
}