namespace Domain.ApplicationUserAggregate
{
    public class RolePermission
    {
        #region Prop 
        public string UserRoleId { get; private set; }
        public int UserPermissionId { get; private set; }

        public bool IsDeleted { get; set; }

        #endregion

        #region Navigtional Properties
        public virtual UserRole UserRole { get; private set; }
        public virtual UserPermission UserPermission { get; private set; }
        #endregion

        #region Ctor
        public RolePermission()
        {

        }
        public RolePermission(string userRoleId, int userPermissionId)
        {
            UserRoleId = userRoleId;
            UserPermissionId = userPermissionId;
            this.IsDeleted = false;
        }
        #endregion

        public void Delete()
        {
            this.IsDeleted = true;
        }
    }
}
