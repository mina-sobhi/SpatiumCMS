namespace Domain.ApplicationUserAggregate
{
    public class UserPermission
    {

        #region Prop 
        public int Id { get; private set; }
        public string Name { get; private set; }
        public int UserModuleId { get; private set; }

        public bool IsDelted { get; set; }
        #endregion

        #region Navigational Properties
        public virtual UserModule UserModule { get; private set; }
        #endregion

        #region Virtual List
        private readonly List<RolePermission> _rolesPermissions = new List<RolePermission>();
        public virtual IReadOnlyList<RolePermission> RolesPermissions => _rolesPermissions.ToList();
        #endregion

        #region Ctor
        public UserPermission()
        {

        }

        public UserPermission(string name, int moduleId)
        {
            Name = name;
            UserModuleId = moduleId;
            this.IsDelted = false;
        }

        public void Delete()
        {
            this.IsDelted = true;
        }

        #endregion 
    }
}
