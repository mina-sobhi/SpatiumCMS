using Domain.ApplicationUserAggregate.Inputs;
using Domain.BlogsAggregate;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using Utilities.Exceptions;
using Utilities.Results;

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
        public bool IsDeleted { get; private set; }
        public int? BlogId { get; private set; }
        #endregion

        #region Navigational Properties
        public virtual ApplicationUser RoleOwner { get; private set; }
        public virtual Blog Blog { get; private set; }
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
            Priority = ++userRoleInput.RoleOwnerPriority;
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
            IconPath = updateInput.IconPath;
            Description = updateInput.Description;

            //change the state of old permission 
            var oldPermissions = _rolePermission.Select(x => x.UserPermissionId).Except(updateInput.PermissionIds);
            foreach (var perm in oldPermissions)
            {
                //reset the old permissions to be deleted 
                var permission = _rolePermission.FirstOrDefault(p => p.UserPermissionId == perm);
                if (!permission.IsDeleted)
                    permission.IsDeleted = true;
            }
            //change the state of current permission
            var CommonPermissions = _rolePermission.Select(x => x.UserPermissionId).Intersect(updateInput.PermissionIds);
            foreach (var perm in CommonPermissions)
            {
                //reset the deleted permissions
                var permission = _rolePermission.FirstOrDefault(p => p.UserPermissionId == perm);
                if (permission.IsDeleted) 
                    permission.IsDeleted = false;
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
/* 
    add new permissions to current role 
    var newrRolePermissions = request.PermissionIds.Except(updateRoleInput.PermissionIds).ToList();
    role.AddPermissions(newrRolePermissions);

    //check if the current permissions found and make it is deleted
    var commonRolePermissions = request.PermissionIds.Intersect(updateRoleInput.PermissionIds).ToList();
    foreach (var permission in commonRolePermissions)
    {
        unitOfWork.RoleRepository.DeleteRolePermission
    }
*/